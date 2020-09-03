Http protocol จะรับส่ง data โดยส่ง request ไปที่ web server เมื่อ web server ได้รับ request จะตรวจสอบสิทธิ์ในการเข้าถึง resource โดย Http protocol จะเป็น stateless (ไม่จดจำสถานะการทำงาน) คือทุกครั้งที่ส่ง request ไปจะไม่มีการจัดเก็บข้อมูลใดๆไว้ที่ web server ทำให้ทุกครั้งที่ส่ง request ไปต้องตรวจสอบสิทธิ์ใหม่ทุกครั้ง แก้ไขปัญหาด้วยวิธีดังนี้

`1. Session`
> เป็นวิธีการที่ web server จดจำสถานะการทำงาน/ระบุตัวตนของ client โดยเมื่อ client ต้องการเข้าถึง resource ของ web server จะสร้างรหัสชุดนึงขึ้นมา (Session Id) แล้วส่ง Session Id ไปพร้อมกับ request 
> - Client จะเก็บ session id เป็น cookies เมื่อมีการส่ง Http request ไปอีกครั้ง client จะต้องส่ง session id ไปด้วยเพื่อบอกกับ web server ว่าเคยได้รับสิทธิ์ในการเข้าถึง resource แล้วจะได้ไม่ต้องตรวจสอบสิทธิ์ใหม่ ทำให้มันกลายเป็น stateful

`2. JWT (Json Web Token)`
> เป็นการทำงานแบบ stateless คือการเอา json data มาทำ Token แล้วส่งไปกับ request ให้ web server จะตรวจสอบสิทธิ์ทุกครั้ง 
>
> `Note :` cookies คือ ข้อมูลขนาดเล็กที่เก็บไว้ที่ web browser พวกข้อมูลการเข้าถึงเว็บไซต์ , ข้อมูลส่วนตัวที่เราใช้ลงทะเบี่ยนในเว็บไซต์
>
> 