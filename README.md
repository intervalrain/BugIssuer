# 簡介
---
> 由於軟體開發的 UAT 期間中，需要收集用戶的使用回饋，該專案便因應而生。
![image](https://github.com/intervalrain/BugIssuer/assets/68344474/bbff4c5f-ade4-4c40-9fe5-3797d5ea8fa0)

## Tech Stack
---
### 後端
+ .NET 8

### 前端
+ ASP.NET Core MVC

### 架構
+ Clean Architecture
  #### Presentation Layer:
    + 使用 ASP.NET Core 來處理用戶界面。
    + 負責 HTTP 請求和回應，包含前端頁面和 API 控制器。
  #### Application Layer:
    + 包含用例 (Use Cases) 和服務 (Services)，處理業務邏輯。
    + 使用 CQRS 模式來區分查詢 (Query) 和命令 (Command)。
  #### Domain Layer:
    + 包含實體 (Entities)、值物件 (Value Objects)、聚合根 (Aggregates) 和領域事件 (Domain Events)。
    + 負責核心業務邏輯和規則。
  #### Infrastructure Layer:
    + 處理資料存取，使用 Entity Framework Core 或 Dapper。
    + 負責外部服務 (例如身份驗證) 的整合。
    + 支援 CQRS 的資料庫操作，如 Command Handler 和 Query Handler。

## 功能需求
---
### 問題回報功能:
+ 用戶可以查看、新建、編輯、刪除問題 (Issue)。
+ 用戶可以在問題下方留下評論 (Comment)。
+ 管理員可以指定受理人 (Assignee)，並更新問題狀態 (Accepted、Pending、Closed)。

### 身份驗證:
+ 使用 Windows 認證來辨識公司內部用戶，確保只有授權用戶能夠存取和操作資料。

### 應用程式切換:
+ 在網頁左上角提供應用程式切換功能，例如從「UEDA」切換到「IEDA」。

### 頁面結構
+ Overview: 顯示新功能或開發者資訊。
+ Version log: 記錄每個版本的修正內容。
+ Issue: 類似 Gitlab Issue 頁面，包含以下元素：
  + Import、Export、New Issue。
  + 頁籤：All、Open、Ongoing、Closed。
  + 搜尋引擎。
  + Issue 清單，包含表頭：Issue Id、Category、Title、Author、DateTime、Last Update、Comments、Assignee、Status。
  + 排序和篩選: 表頭可以點擊排序，也可以篩選。
