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
>1. กำหนด endpoint ของ api ที่ต้องการตรวจสอบ validate token ด้วยการใส่ [Authorize]
>2. ส่ง request เข้ามาที่ endpoint api ที่ติด [Authorize] พร้อมแนบ token ขึ้นมาด้วย 
>  - โดย token ที่ส่งขึ้นมาพร้อม request จะไปอยู่ใน header `Authorization` จะอยู่ในรูปแบบ {Bearer `jwtToken`}\
> `Note :` {Bearer `jwtToken`} มันเป็น stringValue ของ attribute เอามา .First() จะได้ออกมาเป็น string "Bearer `jwtToken`"
>  - เอา jwtToken จาก header `Authorization` มาเช็ค validate  
>3. เช็ค validate Jwt Token
> - 
