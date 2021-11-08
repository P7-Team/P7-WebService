-- #### Create tables ####
-- Users
CREATE TABLE IF NOT EXISTS Users(
    username VARCHAR(50),
    password VARCHAR(255) NOT NULL,
    points   INTEGER DEFAULT 0,
    PRIMARY KEY(username)
);

-- Batch
CREATE SEQUENCE IF NOT EXISTS batch_id START 1;
CREATE TABLE IF NOT EXISTS Batch(
    id              INTEGER DEFAULT nextval('batch_id'),
    description     VARCHAR(255),
    organization    VARCHAR(255),
    link            VARCHAR(255),
    isActive        BOOLEAN DEFAULT FALSE,
    byzantineCheck  BOOLEAN DEFAULT FALSE,
    uploadedOn      TIMESTAMP,
    activatedOn     TIMESTAMP,
    lastWorkedOn    TIMESTAMP,
    ownedBy         VARCHAR(50),
    PRIMARY KEY(id),
    FOREIGN KEY(ownedBy) REFERENCES Users(username)
     ON UPDATE CASCADE
     ON DELETE CASCADE
);

-- Task
CREATE TABLE IF NOT EXISTS Task(
    id INTEGER  UNIQUE,
    number      INTEGER,
    startedOn   TIMESTAMP,
    finishedOn  TIMESTAMP,
    allocatedTo VARCHAR(50),
    PRIMARY KEY(id,number),
    FOREIGN KEY(allocatedTo) REFERENCES Users(username)
     ON UPDATE CASCADE
     ON DELETE SET NULL
);

-- File
CREATE TABLE IF NOT EXISTS File(
    path        VARCHAR(510), -- Linux max is 4096 (seems overkill for our usage)
    filename    VARCHAR(255), -- Linux max is 255
    encoding    VARCHAR(10),
    includedIn  INTEGER,
    PRIMARY KEY(path,filename),
    FOREIGN KEY(includedIn) REFERENCES Batch(id)
     ON UPDATE CASCADE
     ON DELETE CASCADE
);

-- Result
CREATE TABLE IF NOT EXISTS Result(
    path                VARCHAR(510),
    filename            VARCHAR(255),
    isVerified          BOOLEAN DEFAULT FALSE,
    producedById        INTEGER NOT NULL,
    producedByNumber    INTEGER NOT NULL,
    PRIMARY KEY(path,filename),
    FOREIGN KEY(producedById,producedByNumber) REFERENCES Task(id,number)
);

-- Source
CREATE TABLE IF NOT EXISTS Source(
    path        VARCHAR(510),
    filename    VARCHAR(255),
    language    VARCHAR(50) NOT NULL, -- We need to know the language
    PRIMARY KEY(path,filename)
);

-- Argument
CREATE TABLE IF NOT EXISTS Argument(
    path        VARCHAR(510),
    filename    VARCHAR(50),
    number      INTEGER,
    arg         VARCHAR(10) NOT NULL,
    PRIMARY KEY(path,filename,number),
    FOREIGN KEY(path,filename) REFERENCES Source(path,filename)
     ON UPDATE CASCADE
     ON DELETE CASCADE
);

-- Runs
CREATE TABLE IF NOT EXISTS Runs(
    id INTEGER,
    number INTEGER,
    path VARCHAR(510),
    filename VARCHAR(255),
    PRIMARY KEY(id,number,path,filename),
    FOREIGN KEY(id,number) REFERENCES Task(id,number)
     ON UPDATE CASCADE,
    FOREIGN KEY(path,filename) REFERENCES File(path,filename)
     ON UPDATE CASCADE
);

-- #### INDEX ####
-- Automatically created for PRIMARY KEYS in MySql
-- SQL does not provide an IF NOT EXISTS contruct for indecies
CREATE INDEX idx_user_pass_name ON Users (username,password);
CREATE INDEX idx_owned_by ON Batch (ownedBy);
CREATE INDEX idx_allocated_to ON Task (allocatedTo);