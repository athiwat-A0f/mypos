# MyPOS

ระบบ Point of Sale (POS) สำหรับร้านค้าขนาดเล็ก พัฒนาด้วย C# และ WPF  
ใช้สำหรับจัดการการขายสินค้า คำนวณยอดรวม และจัดการรายการสินค้าในบิล

---

## Getting Started

1. Clone repository
git clone https://github.com/yourname/mypos.git

## Features

### Authentication

* ระบบเข้าสู่ระบบผู้ใช้งาน (Login System)
* ตรวจสอบสิทธิ์ก่อนเข้าใช้งานระบบ

### Product Management

* เพิ่ม / แก้ไข / ลบสินค้า (Product CRUD)
* จัดหมวดหมู่สินค้า (Categories)
* เปิด / ปิดสถานะการใช้งานสินค้า
* ค้นหารายการสินค้าได้

### Point of Sale (POS)

* เพิ่มสินค้าเข้าสู่บิลขาย
* ลบสินค้าออกจากบิล
* ปรับจำนวนสินค้า (+ / -)
* แสดงรายการขายแบบ Real-time
* คำนวณราคารวมอัตโนมัติ

### Payment

* หน้าต่างชำระเงิน (Payment Window)
* แสดงยอดรวมสินค้า
* เตรียมรองรับหลายช่องทางการชำระเงิน

### User Interface

* Dialog แจ้งเตือน (Alert Dialog)
* Confirm Dialog สำหรับการยืนยันการทำรายการ
* UI Component ที่นำกลับมาใช้ซ้ำได้

### System Architecture

* แยก Layer ระหว่าง UI / Service / Model
* Service Layer สำหรับจัดการ Business Logic
* Database Service สำหรับเชื่อมต่อ SQL Server

### Future Enhancements

* ระบบจัดการ Stock สินค้า
* ระบบรายงานการขาย (Sales Report)
* ระบบผู้ใช้งานหลายระดับ (Role / Permission)
* ระบบใบเสร็จและการพิมพ์ (Receipt Printing)


---

## Project Structure

```
mypos/
│
├── mystock/
│   │
│   ├── Components/            # Reusable UI components
│   │   ├── AlertDialog.xaml
│   │   └── AlertDialog2.xaml
│   │
│   ├── Helper/                # Utility / helper classes
│   │   └── Alert.cs
│   │
│   ├── Migrations/            # Database migration scripts
│   │
│   ├── Models/                # Data models (Entities)
│   │   ├── Categories.cs
│   │   ├── Products.cs
│   │   └── SaleItem.cs
│   │
│   ├── Pages/                 # Application pages (UI screens)
│   │   ├── LoginPage.xaml
│   │   ├── ProductPage.xaml
│   │   └── SalePage.xaml
│   │
│   ├── Services/              # Business logic / database services
│   │   ├── DbService.cs
│   │   └── ProductService.cs
│   │
│   ├── Views/                 # Additional UI views
│   │
│   ├── App.config             # Application configuration
│   ├── App.xaml               # Application entry UI resources
│   ├── MainWindow.xaml        # Main application window
│   └── PayWindow.xaml         # Payment window
│
└── README.md
```

## Architecture Overview

* **Models** → Data structures used in the application
* **Services** → Database and business logic
* **Pages** → Main UI screens of the application
* **Components** → Reusable UI components (dialogs, widgets)
* **Helper** → Utility helper functions
* **Migrations** → Database migration scripts

## Tech Stack

* **Language:** C#
* **Framework:** WPF (.NET)
* **Database:** SQL Server Express
* **Architecture:** Simple Service Layer

## Application Modules

* Login System
* Product Management (CRUD)
* Sales / POS Screen
* Payment Window
* Alert Dialog System

---


## Author

Developed by Athiwat