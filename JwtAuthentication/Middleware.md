# Middleware
> เป็น pipeline ที่ใช้ handle request , response
>
> `Note : ` `Request delegates (Middleware)` 
> - ใช้สร้าง request pipeline และใช้จัดการแต่ละ Http request ที่เข้ามา
> - ใช้งานผ่าน Method Run , Map , Use
> - สามารถใช้แบบ anonymous(`anonymous middleware`) หรือ ใช้แบบ reusable class(`name middleware`) ทั้งหมดนี้เรียกว่า `"Middleware Component"`
> - แต่ละ Middleware Component ใน request pipeline จะถูกเรียกใช้ต่อกันไปเรื่อยๆ
>
> ![middleware](picture/Middleware.png)
>
> - แต่ละ middleware สามารถจัดการ operation ก่อนหรือหลัง next middleware ได้
> - handle exception  ใน middleware ควรจะถูก call ก่อนใน pipeline และมันสามารถ catch exception ที่เกิดขึ้นในตอนสุดท้ายของ stage ของ pipeline
> - delegate short-circuits (`terminal middleware`) คือ middleware ที่ไม่ได้ส่ง request ไปต่อให้ middleware อื่นๆ
> - `Use` delegate ใช้เชื่อม request delegate หลายๆตัวเข้าด้วยกัน โดยที่ next parameter ก็คือ next delegate ที่จะถูก invoke ต่อใน pipeline
> - delegate short-circuits จะไม่มี next parameter เพราะว่ามันจะไม่ส่ง request ต่อไปให้ delegate อื่น
> - อย่า call `next.invoke()` หลังจากที่ response ถูกส่งไปที่ client แล้ว \
> `Note : `
> - `Run` delegate เป็น short-circuits จึงไม่มีการรับ next parameter จะถูกเอาไว้สุดท้ายเสมอ และทุกครั้งที่ execute เสร็จมันจะ terminate pipeline \
> `Note : ` ไม่ว่าจะเอา `Use` or `Run` delegate ไปไว้ข้างหลัง `Run` delegate มันจะไม่ถูก call เพราะว่า `Run` delegate มันจะ terminate pipeline ทิ้งเสมอ
>
> ![middleware2](picture/Middleware2.png)
>
> - Middleware จะถูกใช้ใน Startup.Configure \
> `1. Exception/error handling` => 
>      - ถ้า app runs ใน Development environment
>        - `UseDeveloperExceptionPage()` => reports app runtime errors
>        - `UseDatabaseErrorPage()` => reports database runtime errors
>      - ถ้า app runs ใน Production environment
>        - `UseExceptionHandler()` => catch และ throw exception
>        - `UseHsts()` => จัดการเกี่ยวกับ Strict-Transport-Security
>
> ![middleware3](picture/Middleware3.png)
>
>      - `UseHttpsRedirection`