
use standardDB

select * from dbo.AspNetRoles

select * from dbo.AspNetUsers order by UserName
select * from dbo.Blogs where UserId = '2a3751aa-33e6-4dda-8f91-3cb980ab51e5'
select * from dbo.BlogComments where UserId = '2a3751aa-33e6-4dda-8f91-3cb980ab51e5'
select * from dbo.BlogLike where UserId = '2a3751aa-33e6-4dda-8f91-3cb980ab51e5'

select * from dbo.AspNetUsers where id = 'ecc59543-c960-4dad-836f-55a8b14781e1'

select * from dbo.UserSelectedLanguages where UserId = 'ecc59543-c960-4dad-836f-55a8b14781e1'

select * from dbo.UploadUserImagesLists where UserId = 'ecc59543-c960-4dad-836f-55a8b14781e1'

select * from dbo.UploadTypes

select * from dbo.Languages

select * from dbo.BlogSourceCategoryNames

select * from dbo.BlogCategories

select * from dbo.UploadUserImagesLists
select * from dbo.UploadBlogImagesList

select * from dbo.Blogs
select * from dbo.Blogs where UserId = ''
select * from dbo.Blogs where Id = '20202'
/*c: 4354d198-25e7-49cd-8b38-dcb1da65405c*/
/* m1: e4309b61-d87f-44d3-9e70-61a9cfad38d0*/
/*m: e4309b61-d87f-44d3-9e70-61a9cfad38d0*/

select * from dbo.BlogComments

select * from dbo.BlogLike

