# OAuth
> `" เป็น standard flow ที่เอาไว้ใช้กับการทำ grant authorization (การอนุญาตสิทธิ์ application ใดๆให้สามารถเข้าถึงข้อมูลของเราจาก application ต้นทางได้) "`

## OAuth Factors
> `1. Resource Owner` => ตัวเรา (เจ้าของ data เช่น รูปภาพ ข้อมูลส่วนตัว ไฟล์ บลาๆ) \
> `2. Resource Server` => Server ที่ใช้เก็บข้อมูล หรือ ให้บริการข้อมูล \
> `3. Client` => application ทั่วๆไป เช่น web app, mobile app, desktop app  \
> `4. Authorization Server` => Server ที่ทำหน้าที่ grant authorize ให้กับ client (เป็นของ Resource Server เช่น ถ้าเราทำการ grant authorize จาก facebook Resource Server ก็คือ facebook ส่วน Authorization Server ก็เป็น server ของ facebook)
>
> `Note : ` การจะทำ grant authorize ได้ client จะต้อง register เข้าไปในระบบของ Resource Server นั้นๆก่อน เพื่อให้ได้ client_id , client_secret มา เช่น ถ้าเราอยากใช้ grant authorize ของ facebook บน grab grab จะต้อง register กับ facebook เพื่อขอ client_id , client_secret `(client_id , client_secret เป็นเหมือน userName , password ของ client เพื่อเข้าใช้งาน Resource Server) `

## Token
> ` " เป็น key(สิทธิ์) ที่ใช้ในการเข้าถึงข้อมูล ณ ช่วงเวลาหนึ่ง มีหมดอายุ ที่ได้จากการ encryption(เข้ารหัส,ถอดรหัส)/hash "` 
>
> `1. Access Token` => Token ที่ใช้สำหรับเข้าถึงข้อมูล (ขอข้อมูล user จาก resource server)
>
> ![accessToken](AccessToken.PNG)
>
> `2. Refresh Token` => Token ที่ใช้สำหรับขอ Access Token ใหม่ เมื่อ Access Token เดิมหมดอายุ
>
> ![refreshToken](RefreshToken.PNG)
>
> `3.Authorization Code` => เป็น Token ที่ได้จากการ Grant Authorize แล้วเอา Token(Authorization Code) มาขอ Access Token อีกที
>
> ![authorizationCode1](AuthorizationCode1.PNG)
>
> ![authorizationCode2](AuthorizationCode2.PNG)
>
> `4.Device Code + User Code` => Token ที่ client ประเภท Smart Device ใช้ขอ Grant Authorize ซึ่ง Token ที่ได้จะเป็น `Device Code + User Code + Verification URI` จาก User เพื่อขอ Access Token
>
> ![deviceCode_UserCode1](DeviceCode_UserCode1.PNG)
>
> ![deviceCode_UserCode2](DeviceCode_UserCode2.PNG)
>
> ![deviceCode_UserCode3](DeviceCode_UserCode3.PNG)
>
> ![deviceCode_UserCode4](DeviceCode_UserCode4.PNG)
>
> ![deviceCode_UserCode5](DeviceCode_UserCode5.PNG)

## Grant Type
> ` "รูปแบบ หรือ วิธีการ grant authorize เพื่อขอ Access Token" `
>
> `1. Authorization Code` => เป็นการ Grant Authorize ที่จะได้ code redirect ข้ามระบบกลับมา เพื่อเอา code ไปขอ Access Token ต่อ มี `2 steps`\
> - client ขอ code 
> - client นำ code ไปขอ Access Token \
>
> ![AuthorizationCode7](AuthorizationCode7.PNG)
>
> ![AuthorizationCode8](AuthorizationCode8.PNG)
>
> ![AuthorizationCode9](AuthorizationCode9.PNG)
>
> ![AuthorizationCode10](AuthorizationCode10.PNG)
>
> Authorization Code แบ่งย่อยได้ 2 แบบ คือ\
> - `Authorization Code (Default)`
>   => ใช้งานกับระบบที่มี Backend Web Server เช่น Facebook , Google , Twitter
> - `Authorization Code PKCE(Proof Key for Code Exchange)`
>   => ใช้งานกับพวก Mobile App , Single Page App , Desktop App เพราะเราไม่สามารถเอา client_secret ไปเก็บไว้บนพวกที่กล่าวมาข้างต้นได้ เลยเกิด Flow PKCE ขึ้นมาเพื่อแก้ปัญหานี้ 
>
> ![AuthorizationCode3](AuthorizationCode3.PNG)
>
> ![AuthorizationCode11](AuthorizationCode11.PNG)
>
> ![AuthorizationCode12](AuthorizationCode12.PNG)
>
> ![AuthorizationCode4](AuthorizationCode4.PNG)
>
> ![AuthorizationCode5](AuthorizationCode5.PNG)
>
> ![AuthorizationCode6](AuthorizationCode6.PNG)
>
> ![AuthorizationCode13](AuthorizationCode13.PNG)
>
> ![AuthorizationCode14](AuthorizationCode14.PNG)
>
> `2. Client Credentials` => เป็นการ Grant Authorize ที่ใช้ `client_id` + `client_secret` ไปขอ Access Token มี `1 step`\
> - client ขอ Access Token ด้วย `client_id` + `client_secret`
>
> ![clientCredentials1](ClientCredentials1.PNG)
>
> ![clientCredentials2](ClientCredentials2.PNG)
>
> `3. Device Code` => เป็นการ Grant Authorize ให้กับพวก Smart Device มี `3 steps`
> - Device ขอ `device_code` + `user_code` + `verification_uri` จาก Authorization Server
> - Device จะเอา `user_code` ที่ได้ไปกรอกใส่ `verification_uri` แล้ว Grant Authorize ได้ Access Token มา
> - Device จะได้ Access Token จาก `device_code` + `user_code`
>
> ![deviceCode_userCode6](DeviceCode_UserCode6.PNG)
>
> ![deviceCode_userCode7](DeviceCode_UserCode7.PNG)
>
> ![deviceCode_userCode8](DeviceCode_UserCode8.PNG)
>
> ![deviceCode_userCode9](DeviceCode_UserCode9.PNG)
>
> `4. Refresh Token` => เป็นการ Grant Authorize เพื่อขอ Access Token ใหม่ ในกรณีที่ Access Token เดิมหมดอายุ มี `1 step`
> - client ขอ Access Token ใหม่ ด้วย Refresh Token
> 
> ![refreshToken1](RefreshToken1.PNG)
>
> ![refreshToken2](RefreshToken2.PNG)
>
> `5. Implicit Token (ไม่ใช้แล้ว)` => เหมือนกับ Authorization Code แต่ return Access Token กลับมาทาง URI ด้วย `fragment(#) ` ทำให้มันมีช่องโหว่ ไม่ปลอดภัย
>
>![implicitToken](ImplicitToken.PNG)
>
> `6. Password Grant (ไม่ใช้แล้ว)` => เป็นการ Grant Authorize โดยใช้ Username,Password เพื่อขอ Access Token การทำแบบนี้จะทำให้ Username + Password ของ User สามารถหลุดไปยัง client อื่นที่เชื่อมต่อกับ Authorization Server ได้
>
>![passwordGrant1](PasswordGrant1.PNG)
>
>![passwordGrant2](PasswordGrant2.PNG)
