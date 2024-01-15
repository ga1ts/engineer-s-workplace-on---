/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2012                    */
/* Created on:     06.08.2020 17:49:59                          */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Employee') and o.name = 'FK_EMPLOYEE_REFERENCE_POST')
alter table Employee
   drop constraint FK_EMPLOYEE_REFERENCE_POST
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Request') and o.name = 'FK_REQUEST_REFERENCE_CLIENT')
alter table Request
   drop constraint FK_REQUEST_REFERENCE_CLIENT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Request') and o.name = 'FK_REQUEST_REFERENCE_EMPLOYEE')
alter table Request
   drop constraint FK_REQUEST_REFERENCE_EMPLOYEE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Request') and o.name = 'FK_REQUEST_REFERENCE_STATUS')
alter table Request
   drop constraint FK_REQUEST_REFERENCE_STATUS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Request') and o.name = 'FK_REQUEST_REFERENCE_FAULTTYP')
alter table Request
   drop constraint FK_REQUEST_REFERENCE_FAULTTYP
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Request') and o.name = 'FK_REQUEST_REFERENCE_TECHNIC')
alter table Request
   drop constraint FK_REQUEST_REFERENCE_TECHNIC
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('RequestWork') and o.name = 'FK_REQUESTW_REFERENCE_SPARE')
alter table RequestWork
   drop constraint FK_REQUESTW_REFERENCE_SPARE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('RequestWork') and o.name = 'FK_REQUESTW_REFERENCE_REQUEST')
alter table RequestWork
   drop constraint FK_REQUESTW_REFERENCE_REQUEST
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('RequestWork') and o.name = 'FK_REQUESTW_REFERENCE_WORK')
alter table RequestWork
   drop constraint FK_REQUESTW_REFERENCE_WORK
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('RequestWork') and o.name = 'FK_REQUESTW_REFERENCE_EMPLOYEE')
alter table RequestWork
   drop constraint FK_REQUESTW_REFERENCE_EMPLOYEE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Spare') and o.name = 'FK_SPARE_REFERENCE_SPARETYP')
alter table Spare
   drop constraint FK_SPARE_REFERENCE_SPARETYP
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SpareParameter') and o.name = 'FK_SPAREPAR_REFERENCE_SPARE')
alter table SpareParameter
   drop constraint FK_SPAREPAR_REFERENCE_SPARE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SpareType') and o.name = 'FK_SPARETYP_REFERENCE_TECHNICT')
alter table SpareType
   drop constraint FK_SPARETYP_REFERENCE_TECHNICT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Technic') and o.name = 'FK_TECHNIC_REFERENCE_MANUFACT')
alter table Technic
   drop constraint FK_TECHNIC_REFERENCE_MANUFACT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Technic') and o.name = 'FK_TECHNIC_REFERENCE_TECHNICT')
alter table Technic
   drop constraint FK_TECHNIC_REFERENCE_TECHNICT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TechnicParameter') and o.name = 'FK_TECHNICP_REFERENCE_TECHNIC')
alter table TechnicParameter
   drop constraint FK_TECHNICP_REFERENCE_TECHNIC
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TechnicParameter') and o.name = 'FK_TECHNICP_REFERENCE_PARAMETE')
alter table TechnicParameter
   drop constraint FK_TECHNICP_REFERENCE_PARAMETE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Client')
            and   name  = 'Index_3'
            and   indid > 0
            and   indid < 255)
   drop index Client.Index_3
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Client')
            and   name  = 'Index_PhoneUnique'
            and   indid > 0
            and   indid < 255)
   drop index Client.Index_PhoneUnique
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Client')
            and   type = 'U')
   drop table Client
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Employee')
            and   name  = 'Index_4'
            and   indid > 0
            and   indid < 255)
   drop index Employee.Index_4
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Employee')
            and   name  = 'Index_3'
            and   indid > 0
            and   indid < 255)
   drop index Employee.Index_3
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Employee')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index Employee.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Employee')
            and   type = 'U')
   drop table Employee
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('FaultType')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index FaultType.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('FaultType')
            and   type = 'U')
   drop table FaultType
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Manufacturer')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index Manufacturer.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Manufacturer')
            and   type = 'U')
   drop table Manufacturer
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Parameter')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index Parameter.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Parameter')
            and   type = 'U')
   drop table Parameter
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Post')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index Post.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Post')
            and   type = 'U')
   drop table Post
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Request')
            and   name  = 'Index_4'
            and   indid > 0
            and   indid < 255)
   drop index Request.Index_4
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Request')
            and   name  = 'Index_3'
            and   indid > 0
            and   indid < 255)
   drop index Request.Index_3
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Request')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index Request.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Request')
            and   type = 'U')
   drop table Request
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('RequestWork')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index RequestWork.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('RequestWork')
            and   type = 'U')
   drop table RequestWork
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Spare')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index Spare.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Spare')
            and   type = 'U')
   drop table Spare
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SpareParameter')
            and   type = 'U')
   drop table SpareParameter
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('SpareType')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index SpareType.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SpareType')
            and   type = 'U')
   drop table SpareType
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Status')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index Status.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Status')
            and   type = 'U')
   drop table Status
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Technic')
            and   name  = 'Index_3'
            and   indid > 0
            and   indid < 255)
   drop index Technic.Index_3
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Technic')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index Technic.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Technic')
            and   type = 'U')
   drop table Technic
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TechnicParameter')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index TechnicParameter.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TechnicParameter')
            and   type = 'U')
   drop table TechnicParameter
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TechnicType')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index TechnicType.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TechnicType')
            and   type = 'U')
   drop table TechnicType
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('Work')
            and   name  = 'Index_2'
            and   indid > 0
            and   indid < 255)
   drop index Work.Index_2
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Work')
            and   type = 'U')
   drop table Work
go

/*==============================================================*/
/* Table: Client                                                */
/*==============================================================*/
create table Client (
   Client_ID            int                  identity,
   FName                varchar(100)         not null,
   ClientName           varchar(50)          not null,
   LName                varchar(100)         not null,
   Phone                varchar(20)          null,
   Email                varchar(50)          null,
   Passport             varchar(150)         null,
   constraint PK_CLIENT primary key (Client_ID)
)
go

/*==============================================================*/
/* Index: Index_PhoneUnique                                     */
/*==============================================================*/




create unique nonclustered index Index_PhoneUnique on Client (Phone ASC)
go

/*==============================================================*/
/* Index: Index_3                                               */
/*==============================================================*/




create unique nonclustered index Index_3 on Client (Email ASC)
go

/*==============================================================*/
/* Table: Employee                                              */
/*==============================================================*/
create table Employee (
   Employee_ID          int                  identity,
   Post_ID              int                  not null,
   FName                varchar(100)         not null,
   EmpName              varchar(50)          not null,
   LName                varchar(150)         not null,
   Phone                varchar(20)          null,
   Email                varchar(50)          null,
   Login                varchar(30)          not null,
   Password             varchar(30)          not null,
   constraint PK_EMPLOYEE primary key (Employee_ID)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create unique nonclustered index Index_2 on Employee (Phone ASC)
go

/*==============================================================*/
/* Index: Index_3                                               */
/*==============================================================*/




create unique nonclustered index Index_3 on Employee (Email ASC)
go

/*==============================================================*/
/* Index: Index_4                                               */
/*==============================================================*/




create unique nonclustered index Index_4 on Employee (Login ASC)
go

/*==============================================================*/
/* Table: FaultType                                             */
/*==============================================================*/
create table FaultType (
   FaultType_ID         int                  identity,
   FaultTypeName        varchar(100)         not null,
   Description          varchar(1000)        null,
   constraint PK_FAULTTYPE primary key (FaultType_ID)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create nonclustered index Index_2 on FaultType (FaultTypeName ASC)
go

/*==============================================================*/
/* Table: Manufacturer                                          */
/*==============================================================*/
create table Manufacturer (
   Manufacturer_ID      int                  identity,
   ManufacturerName     varchar(100)         not null,
   constraint PK_MANUFACTURER primary key (Manufacturer_ID)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create unique nonclustered index Index_2 on Manufacturer (ManufacturerName ASC)
go

/*==============================================================*/
/* Table: Parameter                                             */
/*==============================================================*/
create table Parameter (
   Parameter_ID         int                  identity,
   ParameterName        varchar(100)         not null,
   Description          varchar(1000)        null,
   constraint PK_PARAMETER primary key (Parameter_ID)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create unique nonclustered index Index_2 on Parameter (ParameterName ASC)
go

/*==============================================================*/
/* Table: Post                                                  */
/*==============================================================*/
create table Post (
   Post_ID              int                  identity,
   PostName             varchar(100)         not null,
   constraint PK_POST primary key (Post_ID)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create unique nonclustered index Index_2 on Post (PostName ASC)
go

/*==============================================================*/
/* Table: Request                                               */
/*==============================================================*/
create table Request (
   Request_ID           int                  identity,
   Status_ID            int                  not null,
   FaultType_ID         int                  not null,
   Technic_ID           int                  not null,
   Client_ID            int                  not null,
   Employee_ID          int                  not null,
   RequestDt            datetime             not null,
   SerialNumber         varchar(30)          not null,
   Description          varchar(1000)        not null,
   ExecutionDt          datetime             not null,
   constraint PK_REQUEST primary key (Request_ID)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create nonclustered index Index_2 on Request (SerialNumber ASC)
go

/*==============================================================*/
/* Index: Index_3                                               */
/*==============================================================*/




create nonclustered index Index_3 on Request (RequestDt ASC)
go

/*==============================================================*/
/* Index: Index_4                                               */
/*==============================================================*/




create nonclustered index Index_4 on Request (Client_ID ASC)
go

/*==============================================================*/
/* Table: RequestWork                                           */
/*==============================================================*/
create table RequestWork (
   RequestWork_ID       int                  identity,
   Request_ID           int                  not null,
   Work_ID              int                  not null,
   Employee_ID          int                  not null,
   Spare_ID             int                  null,
   RequestWorkDt        datetime             null,
   constraint PK_REQUESTWORK primary key (RequestWork_ID)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create nonclustered index Index_2 on RequestWork (Request_ID ASC)
go

/*==============================================================*/
/* Table: Spare                                                 */
/*==============================================================*/
create table Spare (
   Spare_ID             int                  identity,
   SpareType_ID         int                  not null,
   Price                money                not null,
   SpareName            varchar(100)         not null,
   Article              varchar(50)          not null,
   constraint PK_SPARE primary key (Spare_ID)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create unique nonclustered index Index_2 on Spare (Article ASC)
go

/*==============================================================*/
/* Table: SpareParameter                                        */
/*==============================================================*/
create table SpareParameter (
   SpareParameter_ID    int                  identity,
   Spare_ID             int                  not null,
   Parameter            varchar(100)         not null,
   ParameterVakue       varchar(50)          not null,
   constraint PK_SPAREPARAMETER primary key (SpareParameter_ID),
   constraint AK_KEY_2_SPAREPAR unique (Spare_ID, Parameter)
)
go

/*==============================================================*/
/* Table: SpareType                                             */
/*==============================================================*/
create table SpareType (
   SpareType_ID         int                  identity,
   TechnicType_ID       int                  not null,
   SpareTypeName        varchar(50)          not null,
   constraint PK_SPARETYPE primary key (SpareType_ID),
   constraint AK_KEY_2_SPARETYP unique (TechnicType_ID, SpareTypeName)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create nonclustered index Index_2 on SpareType (SpareTypeName ASC)
go

/*==============================================================*/
/* Table: Status                                                */
/*==============================================================*/
create table Status (
   Status_ID            int                  identity,
   StatusName           varchar(100)         not null,
   constraint PK_STATUS primary key (Status_ID)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create unique nonclustered index Index_2 on Status (StatusName ASC)
go

/*==============================================================*/
/* Table: Technic                                               */
/*==============================================================*/
create table Technic (
   Technic_ID           int                  identity,
   Manufacturer_ID      int                  not null,
   TechnicType_ID       int                  not null,
   Model                varchar(100)         not null,
   constraint PK_TECHNIC primary key (Technic_ID),
   constraint AK_KEY_2_TECHNIC unique (Manufacturer_ID, Model)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create nonclustered index Index_2 on Technic (Model ASC)
go

/*==============================================================*/
/* Index: Index_3                                               */
/*==============================================================*/




create nonclustered index Index_3 on Technic (Manufacturer_ID ASC)
go

/*==============================================================*/
/* Table: TechnicParameter                                      */
/*==============================================================*/
create table TechnicParameter (
   TechnicParameter_ID  int                  identity,
   Technic_ID           int                  not null,
   Parameter_ID         int                  not null,
   ParameterValue       varchar(200)         not null,
   constraint PK_TECHNICPARAMETER primary key (TechnicParameter_ID),
   constraint AK_KEY_2_TECHNICP unique (Technic_ID, Parameter_ID)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create nonclustered index Index_2 on TechnicParameter (Technic_ID ASC)
go

/*==============================================================*/
/* Table: TechnicType                                           */
/*==============================================================*/
create table TechnicType (
   TechnicType_ID       int                  identity,
   TechnicTypeName      varchar(100)         not null,
   constraint PK_TECHNICTYPE primary key (TechnicType_ID)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create unique nonclustered index Index_2 on TechnicType (TechnicTypeName ASC)
go

/*==============================================================*/
/* Table: Work                                                  */
/*==============================================================*/
create table Work (
   Work_ID              int                  identity,
   WorkName             varchar(100)         not null,
   Description          varchar(1000)        null,
   Price                money                not null,
   constraint PK_WORK primary key (Work_ID)
)
go

/*==============================================================*/
/* Index: Index_2                                               */
/*==============================================================*/




create unique nonclustered index Index_2 on Work (WorkName ASC)
go

alter table Employee
   add constraint FK_EMPLOYEE_REFERENCE_POST foreign key (Post_ID)
      references Post (Post_ID)
go

alter table Request
   add constraint FK_REQUEST_REFERENCE_CLIENT foreign key (Client_ID)
      references Client (Client_ID)
go

alter table Request
   add constraint FK_REQUEST_REFERENCE_EMPLOYEE foreign key (Employee_ID)
      references Employee (Employee_ID)
go

alter table Request
   add constraint FK_REQUEST_REFERENCE_STATUS foreign key (Status_ID)
      references Status (Status_ID)
go

alter table Request
   add constraint FK_REQUEST_REFERENCE_FAULTTYP foreign key (FaultType_ID)
      references FaultType (FaultType_ID)
go

alter table Request
   add constraint FK_REQUEST_REFERENCE_TECHNIC foreign key (Technic_ID)
      references Technic (Technic_ID)
go

alter table RequestWork
   add constraint FK_REQUESTW_REFERENCE_SPARE foreign key (Spare_ID)
      references Spare (Spare_ID)
go

alter table RequestWork
   add constraint FK_REQUESTW_REFERENCE_REQUEST foreign key (Request_ID)
      references Request (Request_ID)
go

alter table RequestWork
   add constraint FK_REQUESTW_REFERENCE_WORK foreign key (Work_ID)
      references Work (Work_ID)
go

alter table RequestWork
   add constraint FK_REQUESTW_REFERENCE_EMPLOYEE foreign key (Employee_ID)
      references Employee (Employee_ID)
go

alter table Spare
   add constraint FK_SPARE_REFERENCE_SPARETYP foreign key (SpareType_ID)
      references SpareType (SpareType_ID)
go

alter table SpareParameter
   add constraint FK_SPAREPAR_REFERENCE_SPARE foreign key (Spare_ID)
      references Spare (Spare_ID)
go

alter table SpareType
   add constraint FK_SPARETYP_REFERENCE_TECHNICT foreign key (TechnicType_ID)
      references TechnicType (TechnicType_ID)
go

alter table Technic
   add constraint FK_TECHNIC_REFERENCE_MANUFACT foreign key (Manufacturer_ID)
      references Manufacturer (Manufacturer_ID)
go

alter table Technic
   add constraint FK_TECHNIC_REFERENCE_TECHNICT foreign key (TechnicType_ID)
      references TechnicType (TechnicType_ID)
go

alter table TechnicParameter
   add constraint FK_TECHNICP_REFERENCE_TECHNIC foreign key (Technic_ID)
      references Technic (Technic_ID)
go

alter table TechnicParameter
   add constraint FK_TECHNICP_REFERENCE_PARAMETE foreign key (Parameter_ID)
      references Parameter (Parameter_ID)
go

