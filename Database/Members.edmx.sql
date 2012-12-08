
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Generated from EDMX file: Members.edmx
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AddressSet'
CREATE TABLE [AddressSet] (
    [Id] integer  NOT NULL,
    [address] nvarchar(2147483647)  NOT NULL,
    [city] nvarchar(2147483647)  NOT NULL,
    [postalcode] nvarchar(2147483647)  NOT NULL,
    [country] nvarchar(2147483647)  NOT NULL
);


-- Creating table 'MemberDetaisSet'
CREATE TABLE [MemberDetaisSet] (
    [Id] integer  NOT NULL,
    [memberno] nvarchar(2147483647)  NOT NULL,
    [memberclass] nvarchar(2147483647)  NOT NULL,
    [membergroup] nvarchar(2147483647)  NOT NULL,
    [memberjob] nvarchar(2147483647)  NOT NULL,
    [joindate] nvarchar(2147483647)  NOT NULL,
    [exitdate] nvarchar(2147483647)  NOT NULL,
    [changeddate] nvarchar(2147483647)  NOT NULL,
    [paymentstatus] nvarchar(2147483647)  NOT NULL
);


-- Creating table 'OrganizationSet'
CREATE TABLE [OrganizationSet] (
    [Id] integer  NOT NULL,
    [key] nvarchar(2147483647)  NOT NULL,
    [value] nvarchar(2147483647)  NOT NULL
);


-- Creating table 'ChangelogSet'
CREATE TABLE [ChangelogSet] (
    [Id] integer  NOT NULL,
    [action] nvarchar(2147483647)  NOT NULL,
    [newvalue] nvarchar(2147483647)  NOT NULL,
    [oldvalue] nvarchar(2147483647)  NOT NULL,
    [memberid] nvarchar(2147483647)  NOT NULL,
    [time] nvarchar(2147483647)  NOT NULL,
    [date] nvarchar(2147483647)  NOT NULL
);


-- Creating table 'MemberSet'
CREATE TABLE [MemberSet] (
    [Id] integer  NOT NULL,
    [firstname] nvarchar(2147483647)  NOT NULL,
    [lastname] nvarchar(2147483647)  NOT NULL,
    [middlenames] nvarchar(2147483647)  NOT NULL,
    [mobile] nvarchar(2147483647)  NOT NULL,
    [email] nvarchar(2147483647)  NOT NULL,
    [phone] nvarchar(2147483647)  NOT NULL,
    [sex] nvarchar(2147483647)  NOT NULL,
    [birthdate] nvarchar(2147483647)  NOT NULL,
    [Address_Id] integer  NOT NULL,
    [MemberDetais_Id] integer  NOT NULL
);


-- Creating table 'ContactSet'
CREATE TABLE [ContactSet] (
    [Id] integer  NOT NULL,
    [firstname] nvarchar(2147483647)  NOT NULL,
    [lastname] nvarchar(2147483647)  NOT NULL,
    [mobile] nvarchar(2147483647)  NOT NULL,
    [email] nvarchar(2147483647)  NOT NULL,
    [phone] nvarchar(2147483647)  NOT NULL,
    [Member_Id] integer  NOT NULL,
    [Address_Id] integer  NOT NULL
);

