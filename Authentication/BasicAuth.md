- Authorize มันเหมือนเป็น guard ป้องกันการเข้าถึง
- Authorize Attribute มันต้องเข้าผ่านตัว middleware authorization ไม่งั้นจะไม่สามารถเข้าถึง
- ต้องเอาไว้ก่อน route middleware เพื่อที่ว่าก่อนจะเข้าถึงแล้ว route ไปต่อได้ต้องผ่านการ authorize ก่อน
- service ต้องรู้จักตัว Handler Authentication

