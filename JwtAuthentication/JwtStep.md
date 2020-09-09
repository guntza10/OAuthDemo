>## Jwt Step
>
> ### `Create Token`
>1. authenticate user
>   - check ก่อนว่ามี user นี้บน database?
>   - ถ้ามีก็จะเอา data ของ user ไป generate Jwt Token
>2. generate Jwt Token
>   - สร้าง JwtHandler ที่เอาไว้จัดการเกี่ยวกับ Jwt Token
>   - เอา SecretKey ของเรามาเข้ารหัส ASCII
>   - สร้าง tokenDescriptor ที่เป็นตัวบอกรายละเอียดที่จำเป็นในการสร้าง Jwt Token
>       - payload
>       - signature
>       - expire
>   - ใช้ JwtHandler ในการ createToken ด้วย tokenDescriptor
>   - ใช้ token ที่ได้จากการ createToken มา writeToken ออกมา
>3. เอา Token ที่ได้แนบไปกับ authenticate response model ที่จะมีข้อมูล User พร้อมกับ token ส่งกลับไป
>
> ### `Validate Token`
>1. กำหนด endpoint ของ api ที่ต้องการตรวจสอบ validate token ด้วยการใส่ [`Authorize`]
>2. ส่ง request เข้ามาที่ endpoint api ที่ติด [Authorize] พร้อมแนบ token ขึ้นมาด้วย โดย JwtMiddleware จะเป็นตัวจัดการกับ request นี้
>  - โดย token ที่ส่งขึ้นมาพร้อม request จะไปอยู่ใน header `Authorization` จะอยู่ในรูปแบบ {Bearer `jwtToken`}\
> `Note :` {Bearer `jwtToken`} มันเป็น stringValue ของ attribute เอามา .First() จะได้ออกมาเป็น string "Bearer `jwtToken`"
>  - เอา jwtToken จาก header `Authorization` มาเช็ค validate  
>3. เช็ค validate Jwt Token
> - ถ้า validate ผ่าน จะไป get เอา Jwt Claim มาตัวนึงที่เป็นข้อมูลของ user เพื่อเอา Jwt Claim นี้ไป get ข้อมูลของ user
> - นำข้อมูลของ `user` แนบไปกับ request header จบการทำงานที่ Jwt Middleware
>4. request ถูกส่งต่อไปที่ Authorization Middleware
> - เราสามารถ custom Authorization Middleware ได้
> - check request ที่เข้ามาผ่าน custom Authorization Middleware ว่ามี header `user` มั้ย ถ้ามีก็จะไม่ทำอะไรก็ส่ง request ผ่านไปต่อสู่ endpoint ของ api ที่ติด [`Authorize`] แต่ถ้าไม่มีก็จะ return message กับ statusCode 401 Unauthorize ออกไป