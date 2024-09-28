Create database doan1chi;
USE doan1chi;



-- Tạo bảng Roles
CREATE TABLE Roles(
	id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    name  ENUM ('user','admin','staff') default('user')
);

-- Tạo bảng AddressDelevery
CREATE TABLE AddressDelivery(
	id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    address  varchar(255)
);

-- Tạo bảng ProductVersioin
CREATE TABLE ProductVersion (
    id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    version varchar(255),
    description varchar(255),
    detail VARCHAR(255),
    price double
);

-- Tạo bảng Images
CREATE TABLE Images(
    id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    linkImage varchar(255)
);

-- Tạo bảng Category
CREATE TABLE Category (
    id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    name VARCHAR(255),
    detail VARCHAR(255),
    isDeleted BOOLEAN DEFAULT 0
);

-- Tạo bảng ActivationCode
CREATE TABLE ActivationCode (
    id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    activeCode VARCHAR(12),
    status ENUM('Active', 'Expired'),
    initDate DATE,
    expiryDate DATE,
    updateDate DaTE,
    periodic ENUM('6', '12', '24') DEFAULT '6',
    price DOUBLE
);


-- Tạo bảng ShippingUnit
CREATE TABLE ShippingUnit (
    id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    name VARCHAR(255),
    address varchar(255)
);


-- Tạo bảng User
CREATE TABLE User (
    id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    account VARCHAR(255) NOT NULL UNIQUE,
    passWord VARCHAR(255),
    idType BOOLEAN DEFAULT 0,
    name VARCHAR(255),
    phone VARCHAR(20),
    image VARCHAR(255),
    gender BOOLEAN,
    address VARCHAR(255),
    birth Date,
    isDeleted BOOLEAN DEFAULT 0,
    idRole INT,
    idAddress int,
    FOREIGN KEY (idRole) REFERENCES Roles(id),
    FOREIGN KEY (idAddress) REFERENCES AddressDelivery(id)
);

-- Tạo bảng Log
CREATE TABLE Log (
    id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    idUser INT,
    activity DATE,
    detail VARCHAR(255),
    FOREIGN KEY (idUser) REFERENCES User(id)
);



-- Tạo bảng Product
CREATE TABLE Product (
    id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    productName VARCHAR(255),
    idCate INT,
    evaluate DOUBLE CHECK (Evaluate >= 0 AND Evaluate <= 5),
    image VARCHAR(255),
    amount INT,
    price DOUBLE,
    detail VARCHAR(255),
    feature VARCHAR(255),
    specifications VARCHAR(255),
    helps VARCHAR(255),
    isDeleted BOOLEAN DEFAULT 0,
    idVersion INT,
    idImage int,
    FOREIGN KEY (idCate) REFERENCES Category(id),
    FOREIGN KEY (idImage) REFERENCES Images(id),
    FOREIGN KEY (idVersion) REFERENCES ProductVersion(id)
);


-- Tạo bảng Orders
CREATE TABLE Orders (
    id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    idUser INT,
    shippingAddress VARCHAR(255),
    orderDate DATE NOT NULL,
    deliveryDate DATE,
    totalAmount int DEFAULT 0,
    totalCost DECIMAL(10,2) DEFAULT 0,
    paymentStatus ENUM('Napas', 'Credit') NOT NULL,
    orderStatus ENUM('Processing', 'Delivered', 'Cancelled', 'Confirmed'),
    FOREIGN KEY (idUser) REFERENCES User(id)
);

-- Tạo bảng Receipt
CREATE TABLE Receipt (
    id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    idOrder INT,
    idShippingUnit INT,
    detail VARCHAR(255),
    date date,
    FOREIGN KEY (idOrder) REFERENCES Orders(id),
    FOREIGN KEY (idShippingUnit) REFERENCES ShippingUnit(id)
);

-- Tạo bảng OrderDetail
CREATE TABLE OrderDetail (
    id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    idOrder int,
    idActiveCode INT,
    idProduct INT,
    productAmount INT DEFAULT 1,
    FOREIGN KEY (idProduct) REFERENCES Product(id),
    FOREIGN KEY (idOrder) REFERENCES Orders(id),
    FOREIGN KEY (idActiveCode) REFERENCES ActivationCode(id)
);
