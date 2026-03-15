# Các quy trình nghiệp vụ
Các cột `CreatedBy`, `CreatedAt` được lấy tự động do người dùng đăng nhập thành công.
## 1. Tạo thông tin sách
```sql
Table Books //Tất cả loại sách
{
  BookID int [primary key]
  CreatedAt time
  CreatedBy int --> Accounts
  Status int
  ---
  Title nvarchar
  Author nvarchar
  Category int --> Categories
  Description nvarchar
  ImageURL varchar
  Price money
}

Table Categories
{
  CategoryID int primary key
  Name nvarchar
}
```
- Nhập sách lẻ: hiển thị như một form các thông tin, Category hiển thị dưới dạng DropDown List các tên thể loại (ẩn ID đi).
- Nhập sách số lượng lớn: hiển hị một DataGrid. 
  - Có thể chỉ đọc ISBN sau đó gọi APT fecth thông tin về hoặc đọc thẳng từ Excel. 
  - Cột Category hiển thị vẫn là DropDown List, nếu chọn thủ công chỉ có thể là những thể loại có sẵn, nếu đọc từ Excel những thể loại không có thì báo lỗi, không tự xóa tên thể loại đi.
  - Categories ID 0 là ID chưa phân loại, mặc định. Các sách bị lỗi có thể set về 0 sau đó thêm Category và chỉnh lại sau.
  - Các sách nhập lỗi sẽ vô hiệu nút nhập. Sẽ có nút nhập hết khi nhập hàng loạt, khi đó những sách lỗi sẽ không được nhập, hoặc không cho nhập hàng loạt nếu có sách bị lỗi.
## 2. Kho lưu và số lượng sách
```sql
--Các vị trí lưu sách trong cửa hàng
Table Stocks 
{
  StockID int pk
  Name nvarchar
  Priority int
  Description nvarchar
}

--Số lượng sách trong kho
Table BookCounts 
{
  BookCountID integer pk
  BookID integer --> Books
  StockID interger --> Stocks
  Count interger
}
```  
- Không quản lý theo kệ, quản lý theo kho và độ ưu tiên, để xử lý việc bán hàng và quản lý số lượng dễ hơn. 
- Số lượng sách trên kệ do quy định từ cửa hàng, nhân viên thấy hết sách phải tự động vào kho mặc định (Priority = 0) lấy sách thêm lên kệ hàng.
- Khi bán hàng, số lượng tự động trừ vào kệ hàng độ ưu tiên nhỏ nhất (0), nếu quản lý theo kệ sẽ rất phiền. Như vậy, số lượng sách trong kho ưu tiên 0 = số lượng thật trong kho + số lượng trưng bày. Những kho ưu tiên != 0 lưu đúng số lượng thật.
- Cảnh báo khi số lượng sách trong kho mặc định nhỏ hơn số nào đó. Nếu kho mặc định còn 0, nhưng những kho khác vẫn còn, trên hệ thống tra cứu thông tin sách vẫn kết ra số lượng > 0. 
- Lúc này khi khách hàng mua hàng thì số lượng trong kho mặc định có thể âm, không tự động trừ vào các kho khác, vẫn phải đảm bảo phần âm không vượt qua số lượng tại các kho khác (đám bảo select sum số lượng luôn >=0)
- Sau khi có cảnh bảo số lượng trong kho mặc định (nhỏ hơn n, 0 hoặc bị âm), các nhân viên phải lập một phiếu chuyển kho để điều chỉnh số lượng sách giữa các kho, hoặc tiến hành nhập sách và lập phiếu nhập nếu tổng số lượng tại các kho không đạt.
```sql
--Thông tin chung của phiếu chuyển
Table StockTransfers 
{
  TransferID int [primary key]
  TransferDate datetime
  CreatedBy int --> Accounts
  Status varchar
  ---
  FromStockID int --> Stocks
  ToStockID int --> Stocks 
  Note nvarchar 
}

-- Danh sách các cuốn sách được chuyển
Table StockTransferDetails 
{
  StockTransferDetails int pk
  TransferID int --> StockTransfers
  BookID int --> Books
  Quantity int 
}
```

## 3. Nhà cung cấp, nhập hàng
```sql
--Nhà cung cấp
Table Suppliers 
{
  SupplierID integer [primary key]
  Name nvarchar
  Address nvarchar
  Email nvarchar
  Phonenumber nvarchar
}

--Phiếu nhập hàng
Table ImportReceipts 
{
  ImportReceiptID integer pk
  CreatedAt time
  CreatedBy integer --> Accounts
  Status int
  ---
  SupplierID integer --> Suppliers
  StockID integer --> Stocks
  TotalAmount money
}

--Chi tiết phiếu nhập hàng
Table ImportDetails 
{
  ImportDetailID integer pk
  ImportReceiptID integer --> ImportReceipts
  ---
  BookID integer --> Books
  Quantity integer
  ImportPrice money
  Note nvarchar
}
```
- Một phiếu nhập hàng nhập các sách từ một nhà cung cấp về cùng 1 kho, nếu cần nhập nhiều nhà cung cấp, nhiều kho thì nhiều phiếu nhập hàng.
- Khi đọc excel nhập hàng, kiểm tra các khóa ngoại trước khi gửi lên database, nếu 1 Books lỗi thì không cho nộp cả phiếu, có thể xử lý bằng các cách sau:
  + Sửa thông tin sách bị lỗi thành sách đã trong danh sách (nếu có)
  + Xóa chi tiết nhập hàng bị lỗi (Không khuyến khích)
  + Thêm popup tạo nhanh tại hàng bị lỗi, quy trình như phần 1

## 4. Khách hàng, tài khoản
```sql
--Khách hàng
Table Customers
{
  CustomerID integer pk
  CreatedAt time
  CreatedBy int --> Accounts

  Phonenumber nvarchar unique
  FullName nvarchar
  Score int
  Dept int
  Address nvarchar [null]
  Email nvarchar [null]
  AccountID integer [null] --> Accounts
}

Table Accounts
{
  AccountID integer pk
  Userame nvarchar unique
  Email nvarchar unique
  PasswordHashed nvarchar
  Role integer //0: admin, 1: staff, 2: customers
}
```
- Tại nhiều cửa hàng, khách hàng không cần tạo tài khoản mà nhân viên hỏi một số thông tin như Tên, Số điện thoại để tiến hành tích điểm mà không cần một Account phức tạp.
- Nếu khách chỉ mua hàng, nhân viên xin thông tin khách và sử dụng cho hóa đơn, Account trong Customer sẽ null.
- Nếu khách hàng tự tạo tài khoản, có cả Account và Customer, lúc này CreatedBy = AccountID = tương ứng với bảng Account đã tạo
- Khi phát sinh dư nợ, có thể bổ sung thêm các thông tin đòi nợ theo quy định cửa hàng (Vẫn không cần tạo Accounts)
- Thông tin nợ trong Customers chỉ nên lưu Nợ hiện tại (Dept), thông tin lưu vết dư nợ sẽ lưu trong bảng MonthlyDebtReports
- Các quy trình cấp AccountID vẫn hoạt động như bình thường theo phân quyền.

## 5. Ưu đãi
- Phân loại 3 ưu đãi:
  - 0: Giảm theo hóa đơn
  - 1: Giảm theo đầu sách
  - 2: Mua hàng tặng hàng

```sql
Table Promotions
{
  PromotionID integer pk
  Code varchar [unique, not null]
  Name nvarchar [not null]       
  CreatedAt datetime [not null]
  CreatedBy integer [not null] --> Accounts
  ---  
  
  PromoType integer [not null]    --0, 1, 2
  --Điều kiện
  MinimumOrderValue money [null]  --0
  RequiredBookID integer [null]   --1, 2 --> Books
  RequiredCategoryID int [null]   --1    --> Categories
  RequiredQuantity integer [null] --1, 2

  --Phần thưởng
  DiscountRate integer [null]     --0, 1, 2
  MaxDiscountAmount money [null]  --0, 1
  GiftBookID integer [null]       --2    --> Books
  GiftQuantity integer [null]     --2

  --Giới hạn
  TotalUsed integer [default: 0] 
  LimitUsage integer [null]       
  StartDate datetime [not null]   
  EndDate datetime [not null]    
  IsActive bit [default: 1]       
}
```
Minh họa một số khuyến mãi
- Giảm 50% cho hóa đơn từ 100k, tối đa 50k
  > Loại 0:  
  > MinimumOrderValue = 100k  
  > DiscountRate = 50%  
  > MaxDiscountAmount = 10k  
- Giảm 10% sách giáo khoa khi mua từ 2 cuốn
  > Loại 1:  
  > RequiredBookID = null  
  > RequiredCategoryID =  SGK  
  > RequiredQuantity = 2  
  > DiscountRate = 10%  
  > MaxDiscountAmount = MAX  
- Mua 1 sách A tặng 1 sách B
  > Loại 2:  
  > RequiredBookID = A  
  > RequiredQuantity = 1  
  > DiscountRate = 100%  
  > GiftBookID = B  
  > GiftQuantity = 1  
- Giảm 50% cho sản phẩm thứ 2 khi mua sách A
  > Loại 2:  
  > RequiredBookID = A  
  > RequiredQuantity = 1  
  > DiscountRate = 50%  
  > GiftBookID = A  
  > GiftQuantity = 1  
- Chưa xử lý được cho combo A+B+C, riêng combo A+B có thể sử dụng loại 2

## 6. Thanh toán
### 6.1. Tạo hóa đơn
```sql
--Hóa đơn
Table Invoices 
{
  InvoiceID integer pk
  CreatedAt time
  CreatedBy int         --> Accounts
  ---
  CustomerID [null] integer    --> Customers
  PromotionID integer   --> Promotions
  DiscountValue money   --from InvoiceDiscountID
  InvoiceValue money    --from  InvoiceDetails
  PaymentStatus integer
}

-- Chi tiết hóa đơn
Table InvoiceDetails
{
  InvoiceDetailID integer pk
  InvoiceID integer   --> Invoices
  BookID integer      --> Books
  Quantity integer
  UnitPrice money     --from Books
  PromotionID int     --> Promotions
  DiscountValue int   --> from promotion
  TotalPrice money --= Q * (UP - DiscountValue)
}
```
- Tại UI thanh toán, lần lượt quét mã hoặc nhập ISBN, hoặc tra cứu tên sách rồi click vào lấy ISBN tạo một dòng tạm.
- Nếu có sách trùng thêm thêm số lượng vào dòng tạm đó
- Liên tục kiểm tra các khuyển mãi loại 1, 2 trên các dòng tạm, quy trình xử lý chi tiết xem bên dưới. Hiện các dropdown chọn ưu đãi, hiện tẩt cả ưu đãi có BookID phù hợp, nhưng ưu đãi hợp lệ thì hiện màu xanh, những ô chưa hợp lệ có thể màu xám và thông báo như "mua thêm 1 cuốn để được giảm 50%".
- Tính toán các khuyến mãi trên từng dòng, cập nhật tổng tiền hóa đơn và kiểm tra khuyển mãi loại 0.
- Trước khi nhấn nút tạo hóa đơn, những thao tác trên chỉ thực hiện trên UI (Hóa đơn ảo), chưa tạo hóa đơn, khi hết hàng cần quét thì nhấn nút tạo hóa đơn
  - Bảng Invoices tạo ra trước, thông tin lấy từ logic đã tính toán, không cần trigger khi thêm các chi tiết hóa đơn
  - Các chi tiết tạo ra sau với mã của hóa đơn vừa tạo, không update trigger do đã tính toán trực tiếp tại FrontEnd
- Bảng hóa đơn Invoices không lưu thông tin thanh toán, mọi thanh toán đều qua phiếu thu.

### 6.2. Quy trình xử lý ưu đãi
- Khuyển mãi loại 1, 2 (đầu sách) thể hiện trong chi tiết hóa đơn, loại 0 thể hiện trong tổng hóa đơn.
- Xử lý khuyến mãi loại 0:
  - Sau khi xử lý hết các khuyến mãi 1 2 trong các chi tiết và tiến hành sum tổng giá trị, hệ thống quét các khuyển mãi loại 0 thỏa yêu cầu, liệt kê DropDown List có gợi ý mã giảm được nhiều nhất (giống shoppe)
  - Tiến hành giảm theo lựa chọn của nhân viên hoặc khách hàng, thêm mã khuyến mãi và số tiền được giảm vào hóa đơn tổng.
  - Tính toán lại giá trị thành tiền của hóa đơn.
- Xử lý khuyến mãi loại 1 và 2:
  - Tại mỗi chi tiết hóa đơn (có 1 BookID), hệ thống tìm các khuyến mãi loại 1 và 2 có BookID (kể cả thỏa và không thỏa để khách hàng biết mua thêm).
  - Tiến hành liệt kê dropdown kể cả thỏa điều kiện và không thỏa điều kiện (BookID buộc phải thỏa).
  - Nếu chọn ưu đãi loại 1, thêm mã ưu đãi vào chi tiết đó.
  - Nếu chọn ưu đãi loại 2: 
    - Tạo thêm 1 chi tiết hóa đơn mới gắn mã khuyến mãi (hóa đơn cũ chỉ để tra cứu). Tại chi tiết mới có thông tin giảm giá, giá mới là 0 nếu mua 1 tặng 1, hoặc giảm 50% sản phẩm thứ 2,...
    - Mỗi khi chi tiết cũ thay đổi số lượng, dòng chi tiết khuyến mãi bị xóa đi, nhân viên phải nhập lại ưu đãi phù hợp hơn

### 6.3. Quy trình thanh toán và phiếu thu
```sql
--Phiếu thu
Table Receipts 
{
  Receipts integer pk
  CreatedAt time
  CreatedBy int            --> Accounts

  CustomerID int [null]    --> Customers
  InvoiceID integer [null] --> Invoices
  ReceiptValue money // != invoice value
}
```
- Thực tế, trước khi nhất nút xuất hóa đơn, nhân viên thực hiện đưa "hóa đơn ảo" cho khách xem và thực hiện thanh toán trước. 
- Có thể hỏi thông tin khách hàng nếu được đồng ý. Nếu cung cấp thì sẽ được tính điểm ưu tiên, không thì trường khách hàng sẽ null. Riêng nếu trả tiền thiếu (nợ) buộc phải hỏi thông tin khách hàng
- Nhân viên tiến thành thu tiền bằng tiền mặt, mã QR. Tại UI, nhân viên nhập số tiền thanh toán và tính ra số tiền thối lại (trả đủ tiền).
- Nếu trả đủ tiền thì nhấn Thanh toán thành công, thực hiện gửi in hóa đơn kèm thông tin thanh toán vừa rồi, sau đó tạo hóa đơn, các chi tiết hóa đơn gửi xuống database thật, thông tin thanh toán cũng được tạo Phiếu thu có khóa ngoại đến hóa đơn vừa tạo và gửi xuống database.
- Nếu trả không đủ tiền:  Bên frontend nếu phát hiện số tiền khách đưa (nhân viên nhập) nhỏ số tiền trên hóa đơn ảo sẽ thông báo tạo nợ, không cho thanh toán thành công khi còn thiếu thông tin khách hàng.
  - Sau khi khách hàng xác nhận nợ, đầy đủ thông tin, nhấn nút thanh toán thành công, tiến hành tạo hóa đơn, phiếu thu như ở trên.
  - Tuy nhiên, khi phát hiện phát sinh nợ, front end ngoài xử lý giao diện cũng phải gửi về backend xử lý nợ Dept của khách hàng. Cứ cập nhập +- dept của khách hàng, việc truy vết đã có bảng báo cáo tháng lo.


## 7. Quản lý công nợ
Các phiếu thu có 2 mục đich:
- Thu tiền cho hóa đơn (Có thể tạo nợ hoặc không, thông báo tạo nợ được gửi từ frontend), InvoiceID không bị null, CustomerID có thể null hoặc không
- Thanh toán nợ cho khách hàng, nếu ông nào đó muốn thanh toán hết nợ, không có hóa đơn, CustomerID buộc phải có, InvoiceID buộc phải null
- Các dữ liệu nợ chỉ update nợ hiện tại của khách hàng, thông tin lưu vết ở trong bảng báo cáo tháng.
  - Khi có giao dịch phát sinh (Khách mua nợ hoặc Khách trả nợ), hệ thống chỉ ghi nhận vào bảng Invoices (Hóa đơn) hoặc Receipts (Phiếu thu). 
  - Đồng thời, hệ thống cộng/trừ trực tiếp vào cột Debt của bảng Customers để thu ngân biết ngay hiện tại khách đang nợ bao nhiêu.

```sql
Table MonthlyDebtReports 
{
  ReportID integer pk
  Month integer
  Year integer
  CustomerID integer --> Customers
  ---
  StartDebt money -- Nợ đầu kỳ
  IncurredDebt money --Phát sinh tăng (Tổng mua nợ trong tháng)
  PaidAmount money --Phát sinh giảm (Tổng trả nợ trong tháng)
  EndDebt money --Nợ cuối kỳ = Start + Incurred - Paid
}
```
- Tại thời điểm cuối tháng, hệ thống quét lấy các CustomerID từ bảng này, danh sách Invoices và Receipts xuất hiện trong tháng, không quét toàn bộ Customer, sau đó truy vấn lấy:
  - StartDebt: truy vấn cùng bảng lấy dư nợ của CustomerID có Month và Year của tháng trước, nếu không tim thấy thì là 0
  - IncurredDebt: Select lấy Sum các nợ từ các hóa đơn Invoices
  - PaidAmount: Sum lấy các Receipts
  - Tính EndDebt, sau đó Insert vào.
- Giải quyết nhu cầu xem theo thời gian khác tháng:
  - Năm: select các tháng trong năm ra tính sum
  - Khoảng thời gian A - B:
    - Nợ đầu kỳ: lấy nợ cuối tháng gần nhất (Nợ gốc) + truy vấn các nợ từ đầu tháng cho đến A
    - Nợ phát sinh: truy vấn các nợ từ A đến B
    - Đã trả: truy vấn các phiếu thu từ A đến B
    - Nợ hiện tại: công thức

## 8. Hàng tồn
- Khi thêm một chi tiết hóa đơn, một chi tiết nhập hàng, một chi tiết chuyển kho, số lượng sách sẽ được update ở BookCount, thông tin lưu vết thì lưu ở bảng dưới đây theo mỗi tháng, giống ở trên

```sql
Table MonthlyStockReports 
{
  ReportID integer pk
  Month integer
  Year integer
  StockID integer --> Stocks (Kho/Cửa hàng)
  BookID integer --> Books
  ---
  StartCount integer  -- Tồn đầu kỳ
  ImportCount integer -- Tổng Nhập (Nhập NCC + Chuyển kho đến)
  ExportCount integer -- Tổng Xuất (Bán hàng + Chuyển kho đi)
  EndCount integer    -- Tồn cuối kỳ = StartCount + ImportCount - ExportCount
}
```
- Quy tắc hoạt động giống báo cáo nợ:
  - StartCount truy vấn cùng bảng EndCount của tháng trước haowcj = 0
  - ImportCount: sum các ImportDetails (nhập) và StockTransferDetails (chuyển đến)
  - ExportCount: sum các InvoiceDetails (chi tiết hóa đơn) và StockTransferDetails (chuyển đi)
  - EndCount: áp dụng công thức