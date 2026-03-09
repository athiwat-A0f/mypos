# MyPOS

ระบบ Point of Sale (POS) สำหรับร้านค้าขนาดเล็ก พัฒนาด้วย C# และ WPF  
ใช้สำหรับจัดการการขายสินค้า คำนวณยอดรวม และจัดการรายการสินค้าในบิล

---

## Getting Started

1. Clone repository
git clone https://github.com/yourname/mypos.git

## Features

- ระบบ Login ผู้ใช้งาน
- เพิ่ม / ลบสินค้าในบิล
- ปรับจำนวนสินค้า (+ / -)
- คำนวณราคารวมอัตโนมัติ
- แสดงรายการขายแบบ Real-time
- รองรับการพัฒนาเพิ่มเติม เช่น Stock และ Report

---

## Technology

- C#
- WPF (.NET 10)
- SQL Server (planned)
- MVVM (planned)

---

## Project Structure
mypos
│
├── MainWindow.xaml # Layout หลักของระบบ
├── LoginPage.xaml # หน้า Login
├── SalePage.xaml # หน้าขายสินค้า
│
├── Models
│ └── SaleItem.cs # โครงสร้างข้อมูลสินค้า
│
├── Services
│ └── SaleService.cs
│
└── Assets
---

## Future Improvements

- Barcode Scanner
- Receipt Printing
- Stock Management
- Sales Report
- Database Integration

---

## Author

Developed by Athiwat