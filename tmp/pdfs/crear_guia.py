from reportlab.platypus import SimpleDocTemplate, Paragraph, Spacer, Table, TableStyle, PageBreak
from reportlab.lib.styles import getSampleStyleSheet
from reportlab.lib import colors
from reportlab.lib.enums import TA_LEFT
from reportlab.graphics.shapes import Drawing, Rect, String, Line, Polygon
from pathlib import Path
from xml.sax.saxutils import escape
import re
root=Path.cwd()
styles=getSampleStyleSheet()
styles['Normal'].fontSize=9.5
styles['Normal'].leading=13
styles['Normal'].spaceAfter=5
styles['Heading1'].fontSize=21
styles['Heading1'].leading=26
styles['Heading2'].spaceBefore=14
navy=colors.HexColor('#203c50')
for name in ['Title','Heading1','Heading2','Heading3']: styles[name].textColor=navy

def box(d,x,y,w,h,title,lines):
 d.add(Rect(x,y,w,h,fillColor=colors.HexColor('#f0f5f8'),strokeColor=navy))
 d.add(String(x+9,y+h-18,title,fontName='Helvetica-Bold',fontSize=10,fillColor=navy))
 d.add(Line(x,y+h-26,x+w,y+h-26,strokeColor=navy))
 for i,t in enumerate(lines): d.add(String(x+9,y+h-42-i*14,t,fontSize=9))
def arrow(d,x1,y1,x2,y2,label=''):
 d.add(Line(x1,y1,x2,y2,strokeColor=navy))
 import math
 a=math.atan2(y2-y1,x2-x1)
 pts=[x2,y2,x2-7*math.cos(a-.4),y2-7*math.sin(a-.4),x2-7*math.cos(a+.4),y2-7*math.sin(a+.4)]
 d.add(Polygon(pts,fillColor=navy,strokeColor=navy))
 if label:d.add(String((x1+x2)/2+4,(y1+y2)/2+5,label,fontSize=8))
def flow():
 d=Drawing(490,205)
 box(d,5,120,125,65,'Web',['Pantallas Web Forms'])
 box(d,180,120,125,65,'Datos',['Consultas SQL'])
 box(d,355,120,125,65,'Dominio',['Clases del negocio'])
 arrow(d,130,153,180,153);arrow(d,305,153,355,153)
 d.add(Line(65,185,65,200));d.add(Line(65,200,415,200));arrow(d,415,200,415,185)
 box(d,180,5,125,65,'SQL Server',['BarberiaDB'])
 arrow(d,242,120,242,70)
 return d

def classes():
 d=Drawing(490,520)
 box(d,5,375,145,135,'Cliente',['Id: int','Nombre: string','Apellido: string','Telefono: string'])
 box(d,172,375,145,135,'Barbero',['Id: int','Nombre: string','Apellido: string'])
 box(d,339,375,145,135,'Servicio',['Id: int','Nombre: string','Precio: decimal','DuracionMinutos: int'])
 box(d,150,135,190,165,'Turno',['Id: int','FechaHora: DateTime','Cliente: Cliente','Barbero: Barbero','Servicio: Servicio','Estado: EstadoTurno'])
 for x,target in [(77,180),(244,244),(410,310)]:
  d.add(Line(x,375,target,300,strokeColor=navy))
  d.add(String(x+5,360,'1',fontSize=9));d.add(String(target+5,307,'0..*',fontSize=9))
 box(d,5,0,185,115,'EstadoTurno (enum)',['Pendiente / Confirmado','Cancelado / Atendido','NoAsistio'])
 box(d,300,0,184,90,'ServicioDatos',['Listar(): List<Servicio>'])
 arrow(d,175,135,130,115,'usa')
 d.add(String(300,102,'Devuelve objetos Servicio',fontSize=9))
 return d

def fmt(t):
 t=escape(t.replace('→', ' - '))
 t=re.sub(r'\*\*(.*?)\*\*',r'<b>\1</b>',t)
 t=re.sub(r'`([^`]+)`',r'<font name="Courier">\1</font>',t)
 return t
lines=(root/'README.md').read_text(encoding='utf-8-sig').splitlines()
story=[];i=0;diagram=0
while i<len(lines):
 s=lines[i].strip();i+=1
 if not s:continue
 if s.startswith('```'):
  lang=s[3:];block=[]
  while i<len(lines) and not lines[i].strip().startswith('```'): block.append(lines[i]);i+=1
  i+=1
  if lang=='mermaid':
   diagram+=1
   if diagram==2:pass
   story.append(flow() if diagram==1 else classes())
  else:
   for ln in block:story.append(Paragraph(escape(ln),styles['Code']))
   story.append(Spacer(1,10))
 elif s.startswith('|'):
  rows=[s]
  while i<len(lines) and lines[i].strip().startswith('|'):rows.append(lines[i].strip());i+=1
  data=[[Paragraph(fmt(c.strip()),styles['Normal']) for c in r.strip('|').split('|')] for r in rows if not re.match(r'^\|[\s:|\-]+\|$',r)]
  table=Table(data,colWidths=[95,395]);table.setStyle(TableStyle([('BACKGROUND',(0,0),(-1,0),colors.HexColor('#e1eaf0')),('GRID',(0,0),(-1,-1),.5,colors.HexColor('#ccd5dd')),('VALIGN',(0,0),(-1,-1),'TOP'),('LEFTPADDING',(0,0),(-1,-1),8)]));story.append(table)
 elif s.startswith('# '):story.append(Paragraph(fmt(s[2:]),styles['Heading1']))
 elif s.startswith('## '):
  if 'Cómo leemos' in s or 'Clases y relaciones' in s:story.append(PageBreak())
  story.append(Paragraph(fmt(s[3:]),styles['Heading2']))
 elif s.startswith('### '):story.append(Paragraph(fmt(s[4:]),styles['Heading3']))
 else:
  if s.startswith('- '):s='• '+s[2:]
  story.append(Paragraph(fmt(s),styles['Normal']))

def footer(c,doc):
 c.setFont('Helvetica',8);c.setFillColor(navy)
 c.drawString(42,25,'Barbería | Guía del equipo | Hito 1')
 c.drawRightString(553,25,str(doc.page))
out=root/'output/pdf/Guia del equipo Barberia.pdf'
SimpleDocTemplate(str(out),pagesize=(595.28,841.89),rightMargin=45,leftMargin=45,topMargin=36,bottomMargin=42,title='Sistema de turnos para barbería - Guía del equipo',author='Equipo Barbería').build(story,onFirstPage=footer,onLaterPages=footer)
print(out)

