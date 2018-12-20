INSERT INTO [dbo].[Train_Menu]
           ([Menu_Id]
           ,[Menu_Title]
           ,[Menu_Url]
           ,[Menu_FatherId]
           ,[Status]
           ,[Create_Time]
           ,[Create_User]
           ,[Update_Time]
           ,[Update_User]
           ,[Menu_Order])
     VALUES
           (NEWID()
           ,'∂©µ•π‹¿Ì'
           ,'/Admin/Order/Index'
           ,'85254502-8629-469F-9427-B110D0F3B2BF'
           ,1
           ,'2018-10-23'
           ,'admin'
           ,'2018-10-23'
           ,'admin'
           ,'4');
		   
		   
		   
		   alter table Train_Article alter column ArticleTitle nvarchar(500);
		   
		   
		   alter table Train_Order add WXPayOutTradeNumber nvarchar(50);
alter table Train_Order add WXPayOutTradeTime datetime;
alter table Train_Order add WXPayUnifyInfo nvarchar(max);







