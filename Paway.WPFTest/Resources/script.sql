


-- ----------------------------
-- Table structure for Organ_Auths
-- Date: 2022-11-11
-- ----------------------------
CREATE TABLE [Organ_Auths](
"Id"  integer Primary Key AutoIncrement not null,
"ParentId" int,
"MenuType" int,
"ButtonType" int,
"CreateOn"  datetime,
"UpdateOn"  datetime,
 unique(Id asc)
);
GO
Create index main.Organ_Auths_id on Organ_Auths (Id ASC);
GO

-- ----------------------------
-- Table structure for Sys_Users
-- Date: 2011-11-11
-- ----------------------------
CREATE TABLE [Sys_Users](
"Id"  integer Primary Key AutoIncrement not null,
"ParentId"  int,
"Name"  nvarchar(32),
"Display"  nvarchar(32),
"Password"  nvarchar(32),
"Enable"  int,
"LoginOn"  datetime,
"CreateOn"  datetime,
"UpdateOn"  datetime,

"Sex" int,
"UserType"  int,
 unique(Id asc)
);
GO
Create index main.Sys_Users_id on Sys_Users (Id ASC);
GO

-- ----------------------------
-- Table structure for Sys_Sys_Admins
-- Date: 2011-11-11
-- ----------------------------
CREATE TABLE [Sys_Admins](
"Id"  integer Primary Key AutoIncrement not null,
"Name"  nvarchar(255),
"Value"  nvarchar(255),
"CreateOn"  datetime,
"UpdateOn"  datetime,
 unique(Id asc)
);
GO
Create index main.Sys_Admins_id on Sys_Admins (Id ASC);
GO