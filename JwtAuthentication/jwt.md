# Asp.Net Core Jwt Authentication Structure

> ## Controller
> ใช้กำหนด endpoint,route สำหรับ Web Api เป็น entry point ของ Web Api ที่ client สามารถเข้าถึงผ่าน request
>
> `Note :` endpoint คือ ulr ที่จะใช้ call api

> ## Entity
>  คือ model ของ database ที่ใช้ส่ง data ระหว่าง service กับ controller
>
> `Note :` [JsonIgnore] คือ attribute ที่ป้องกัน property จากการ serialize และป้องกันไม่ให้ return ไปกับ response
>
> ![entity](picture/Entity.png)

> ## AppSettings
> คือ class model ที่เอาไว้ map กับ property ที่ถูก define ไว้ใน `appsettings.json`
>
> ![appsettings](picture/Appsettings.png)
> ![appsettingsJson](picture/AppsettingsJson.png)

> ## Custom Authorize Attribute
> คือการ custom Authorize Attribute ที่ใช้ในการจำกัดสิทธิ์ในการเข้าถึงแต่ละ endpoint
>
> ![AuthorizeAttribute](picture/AuthorizeAttribute.png)
>
> จะทำงานผ่าน OnAuthorization โดยมันจะ check ว่ามี authenticated user ถูกส่งมากับ current request มั้ย (`context.HttpContext.Items["User"]`) ถ้ามีก็จะไม่ทำอะไรแล้วส่ง request ต่อไปที่ controller แต่ถ้าไม่มี มันจะ return response 401 Unauthorized กลับไป
>
> `Note : ` ถ้ามี authenticated user ถูกส่งมากับ current request มันก็จะถูกส่งผ่าน Jwt Middleware (`ถ้า request มี valid JWT access token`)

> ## JWT Middleware
> `Note : ` Middleware คือ request pipeline ที่เอาไว้ใช้ประโยชน์ดังนี้
> - เชื่อมต่อ application กับ network
> - จัดการ request/response
> - จัดการ/กรอง request ที่เราต้องการ
> - สามารถจัดการให้ระบบเสถียรและสามารถรองรับข้อมูลได้ตลอดเวลา
>
> JWT Middleware จะจัดการกับ token ดังนี้
> - validate token
> - แตก userId ออกจาก Token 
> - แนบ authenticated user เข้ามาใน current _`HttpContext.Items`_
>
> ถ้าไม่มี token หรือ ขั้นตอนด้านบน fail แล้วไม่มี user แนบมากับ http context จะ return response 401 Unauthorized กลับไป

> ## Authenticate Request Model
> เป็น model สำหรับ request ที่ใช้คุยกับฝั่ง client 
>
> ![authenticateRequestModel](picture/authenticateRequestModel.png)
>
> `Note :` [`Required`] attribute เอาไว้ทำ validate ถ้าไม่มีข้อมูลส่งมากับ request body มันจะแจ้ง error ตอนที่ call api 

> ## Authenticate Response Model
> เป็น model สำหรับ response ที่ใช้คุยกับฝั่ง client ซึ่งจะประกอบไปด้วยข้อมูลของ user ยกเว้น password กับ token 
>
> ![authenticateReponseModel](picture/authenticateReponseModel.png)

> ## UserService
> จะเอาไว้จัดการ authenticate user , return token , get all user , get use by id