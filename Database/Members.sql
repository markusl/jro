
-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- These are the table creation queries.
-- Create new queries:
-- 1. Open Members.edmx
-- 2. Generate Database from Model
-- 3. Members.edmx.sql is created
-- 4. Strip out unnecessary parts and create this file
-- 5. Add to DatabaseSettings.settings to be used from code
-- -05/2010 ml

-- Creating table 'AddressSet'
CREATE TABLE [AddressSet] (
    [Id] int  NOT NULL,
    [address] nvarchar(2147483647)  NOT NULL,
    [city] nvarchar(2147483647)  NOT NULL,
    [postalcode] nvarchar(2147483647)  NOT NULL,
    [country] nvarchar(2147483647)  NOT NULL
);

-- Creating table 'MemberDetaisSet'
CREATE TABLE [MemberDetaisSet] (
    [Id] int  NOT NULL,
    [memberno] nvarchar(2147483647)  NOT NULL,
    [memberclass] nvarchar(2147483647)  NOT NULL,
    [membergroup] nvarchar(2147483647)  NOT NULL,
    [memberjob] nvarchar(2147483647)  NOT NULL,
    [joindate] nvarchar(2147483647)  NOT NULL,
    [exitdate] nvarchar(2147483647)  NOT NULL,
    [changeddate] nvarchar(2147483647)  NOT NULL
);

-- Creating table 'OrganizationSet'
CREATE TABLE [OrganizationSet] (
    [Id] int  NOT NULL,
    [key] nvarchar(2147483647)  NOT NULL,
    [value] nvarchar(2147483647)  NOT NULL
);

-- Creating table 'ChangelogSet'
CREATE TABLE [ChangelogSet] (
    [Id] int  NOT NULL,
    [action] nvarchar(2147483647)  NOT NULL,
    [newvalue] nvarchar(2147483647)  NOT NULL,
    [oldvalue] nvarchar(2147483647)  NOT NULL,
    [memberid] nvarchar(2147483647)  NOT NULL,
    [time] nvarchar(2147483647)  NOT NULL,
    [date] nvarchar(2147483647)  NOT NULL
);

-- Creating table 'MemberSet'
CREATE TABLE [MemberSet] (
    [Id] int  NOT NULL,
    [firstname] nvarchar(2147483647)  NOT NULL,
    [lastname] nvarchar(2147483647)  NOT NULL,
    [middlenames] nvarchar(2147483647)  NOT NULL,
    [mobile] nvarchar(2147483647)  NOT NULL,
    [email] nvarchar(2147483647)  NOT NULL,
    [phone] nvarchar(2147483647)  NOT NULL,
    [sex] nvarchar(2147483647)  NOT NULL,
    [birthdate] nvarchar(2147483647)  NOT NULL,
    [Address_Id] int  NOT NULL,
    [MemberDetais_Id] int  NOT NULL
);

-- Creating table 'ContactSet'
CREATE TABLE [ContactSet] (
    [Id] int  NOT NULL,
    [firstname] nvarchar(2147483647)  NOT NULL,
    [lastname] nvarchar(2147483647)  NOT NULL,
    [middlenames] nvarchar(2147483647)  NOT NULL,
    [mobile] nvarchar(2147483647)  NOT NULL,
    [email] nvarchar(2147483647)  NOT NULL,
    [phone] nvarchar(2147483647)  NOT NULL,
    [sex] nvarchar(2147483647)  NOT NULL,
    [birthdate] nvarchar(2147483647)  NOT NULL,
    [Member_Id] int  NOT NULL,
    [Address_Id] int  NOT NULL
);
