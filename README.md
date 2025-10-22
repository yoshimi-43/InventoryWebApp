# 在庫管理システム

## 概要
本システムは、商品の在庫を管理するためのWebアプリケーションです。

## 1. 要件定義

### 1.1 システム概要
- 商品の在庫を管理するWebアプリケーション
- 商品の登録、編集、削除、一覧表示が可能
- 商品データのCSVエクスポート機能あり

### 1.2 機能要件
1. 商品管理機能
   - 商品の新規登録
   - 商品情報の編集
   - 商品の削除
   - 商品一覧の表示（ページング機能付き）
   - 商品の検索機能
   - 商品データのCSVエクスポート

2. 商品データ項目
   - ID（自動採番）
   - 商品名（必須）
   - 数量（0以上の整数）
   - 単価（1円以上の整数）
   - 合計金額（数量×単価の自動計算）

### 1.3 非機能要件
- レスポンシブデザイン対応
- SQLiteデータベースを使用
- ASP.NET Core MVCフレームワークを使用

## 2. 基本設計

### 2.1 システム構成
- フロントエンド: HTML, CSS (Bootstrap), JavaScript (jQuery)
- バックエンド: C# (.NET 8.0)
- データベース: SQLite
- 開発環境: Visual Studio Code

### 2.2 データベース設計

#### Products テーブル
| カラム名 | データ型 | 説明 |
|----------|----------|------|
| ProductId | int | 主キー |
| Name | string | 商品名 |
| Quantity | int | 在庫数 |
| Price | int | 単価 |
| TotalValue | int | 合計金額 |

### 2.3 画面設計
#### 商品一覧画面
*   商品の一覧表示（ページング付き）
*   検索ボックス
*   新規登録ボタン
*   CSVエクスポートボタン
*   各商品の編集/詳細/削除ボタン

#### 商品登録/編集画面
*   商品名入力フィールド
*   数量入力フィールド
*   単価入力フィールド
*   保存/キャンセルボタン

#### 商品詳細画面
*   商品情報の表示
*   戻るボタン

#### 削除確認画面
*   削除確認メッセージ
*   削除/キャンセルボタン

## 3. 詳細設計

### 3.1 ルーティング一覧
#### 商品管理
- GET /Products - 商品一覧画面取得
- GET /Products/Create - 商品登録画面取得
- POST /Products/Create - 商品登録
- GET /Products/Details/{id} - 商品詳細
- GET /Products/Edit/{id} - 商品編集画面取得
- PUT /Products/Edit/{id} - 商品更新
- GET /Products/Delete/{id} - 商品削除確認画面取得
- DELETE /Products/Delete/{id} - 商品削除

### 3.2 主要コンポーネント
#### Models/Product.cs
*   商品情報を表すモデルクラス
*   バリデーション属性による入力チェック
*   合計金額の自動計算プロパティ

#### Models/PagedResult.cs
*   ページング機能用のジェネリッククラス
*   商品一覧の表示に使用

#### Repositories/ProductRepository.cs
*   SQLiteデータベースとの通信を担当
*   Dapperを使用したCRUD操作の実装
*   ページング機能付きデータ取得

#### Controllers/ProductsController.cs
*   商品管理の主要なロジックを実装
*   画面遷移の制御
*   CSVエクスポート機能

### 3.3 主要機能の実装詳細
#### 検索機能
*   Ajax通信による非同期検索
*   部分一致での商品名検索

#### ページング機能
*   1ページあたり10件表示
*   前後ページへの移動ボタン
*   現在のページ番号表示

#### CSVエクスポート
*   UTF-8エンコーディング
*   カンマ区切りフォーマット
*   ダブルクオート特殊文字のエスケープ処理

### 3.4 セキュリティ対策
*   XSS対策：Razor構文によるHTMLエスケープ
*   CSRF対策：AntiForgeryToken
*   SQLインジェクション対策：Dapperのパラメータ化

## 4. 開発環境・動作要件
#### 4.1 開発環境
*   Visual Studio Code
*   .NET 8.0 SDK
*   Git/GitHub

#### 4.2 使用パッケージ
*   Microsoft.Data.Sqlite
*   Dapper

#### 4.3 動作要件
*   OS: Windows/macOS/Linux
*   .NET 8.0 Runtime
*   モダンブラウザ（Chrome/Firefox/Edge等）
