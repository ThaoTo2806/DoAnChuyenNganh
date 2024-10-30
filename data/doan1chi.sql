create database doan1chi;

use doan1chi;

create table Roles(
	ID int primary key auto_increment,
    Name varchar(255),
    IsActive boolean
);
INSERT INTO Roles (Name, IsActive)
VALUES
    ('admin', true),
    ('user', false);


create table User(
	idUser int primary key auto_increment,
    Account varchar(255),
    Password varchar(255),
    Email varchar(255),
    Phone varchar(15),
    Address varchar(255),
    Gender boolean,
    Birth date,
    IdRole	int,
    IsDeleted boolean,
    
    foreign key (IdRole) references Roles(ID)
);

ALTER TABLE `User`
ALTER IdRole SET DEFAULT 2;

ALTER TABLE `User`
ALTER IsDeleted SET DEFAULT 0;

ALTER TABLE `User`
ADD CONSTRAINT UC_Account UNIQUE (Account);


INSERT INTO `User` (`idUser`, `account`, `Password`, `IdRole`, `Username`, `Phone`, `Gender`, `Address`, `IsDeleted`, `Email`, `Birth`) 
VALUES
(1, 'admin', '1', 1, 'Thao', '1111111111', 1, '123 abs', 0, 'thaonguyen28062003@gmail.com', '1996-07-10'),
(2, 'user', '2', 1, 'Thao', '0123456789', 0, '140 Le Trong Tan', 0, 'ln5966220@gmail.com', '2003-07-10'),
(3, 'cay', '1', 2, 'Tri Cuong', '1111111111', 0, '17B Tan Tru', 0, 'letricuong08@gmail.com', '1996-07-25'),
(4, 'daiuy', '1', 2, 'Tom', '1111111111', 0, '189 Le Trong Tan', 0, 'daiuyton@gmail.com', '1996-07-10'),
(5, 'dola', '1', 2, 'Cong Cao', '1111111111', 0, '17B Tan Tru', 0, 'congcao2903@gmail.com', '1996-07-10'),
(6, 'abc', '1', 2, 'Trong Nhan', '1111111111', 0, '140 Le Trong Tan', 0, 'trong43@gmail.com', '1996-07-10'),
(7, 'a', '1', 2, 'A', '1111111111', 1, '1223 abd', 0, 'Hi@gmail.com', '1996-07-10'),
(8, 'hi', '1', 2, 'Hi', '1111111111', 1, 'abg', 0, 'hi@gmail.com', '1996-07-10'),
(9, 'kkk', '1', 2, 'afh', '1234567898', 1, 'akd', 0, 'shak@gmail.com', '2024-07-04'),
(10, 'thaoto', '1', 2, 'Thao', '1234567899', 0, 'aa', 0, 'thao@gmail.com', '1924-01-01'),
(15, 'thanh29', '1', 2, 'Thanh', '0123456789', NULL, NULL, 0, 'a@gmail.com', '0001-01-01'),
(16, 'nanh3495', 'jmsCdP9AbzEH', 2, 'Ngoc Anh', '0123456789', 1, 'abc dbf', 0, 'ln5966220@gmail.com', '1994-07-23'),
(17, 'bin390', 'rFEBgzi9MIlY', 2, 'BinBin', '0123456789', 1, '123 fkh', 0, 'ln5966220@gmail.com', '1990-07-23'),
(18, 'toto', 'XR5Grm1WmnTG', 2, 'Thao To', '0123456789', 0, 'abc dbj', 0, 'ln5966220@gmail.com', '1974-06-18'),
(19, 'to98374', '1', 2, 'thao', '0123456798', 1, '123 ffff', 0, 'thao@gmail.com', '2004-07-24'),
(20, 'nah309', 's4ZTtvEshsVE', 2, 'Ngoc Anh', '0123456789', 0, '124 Truong Chinh', 0, 'ln5966220@gmail.com', '1957-11-21'),
(21, 'ttt1', '1', 1, 'thanh thao', '0123456789', NULL, NULL, 0, 'ln5966220@gmail.com', NULL),
(22, 'thao123', 'z7B3LBJrZ5S3', 2, 'thanh thao', '0123456789', 0, 'a', 0, 'ln5966220@gmail.com', '1925-06-09');


create table Log(
	ID int primary key auto_increment,
    IdMember int,
    Activity date,
    Detail text,
    
    foreign key (IdMember) references User(idUser)
);

INSERT INTO `log` (`ID`, `IdMember`, `Activity`, `Detail`) VALUES
(44, 1, '2024-07-03 17:59:03', 'Login to System'),
(45, 1, '2024-07-03 18:00:31', 'Login to System'),
(46, 1, '2024-07-04 11:51:05', 'Login to System'),
(47, 1, '2024-07-04 11:54:13', 'Login to System'),
(48, 1, '2024-07-04 11:56:40', 'Login to System'),
(49, 1, '2024-07-04 11:59:40', 'Login to System'),
(50, 1, '2024-07-04 12:00:47', 'Login to System'),
(51, 1, '2024-07-04 12:05:20', 'Login to System'),
(52, 1, '2024-07-04 12:05:20', 'Login to System'),
(53, 1, '2024-07-04 12:05:20', 'Login to System'),
(54, 1, '2024-07-04 12:06:14', 'Login to System'),
(55, 1, '2024-07-04 12:12:03', 'Login to System'),
(56, 1, '2024-07-04 12:17:14', 'Login to System'),
(57, 1, '2024-07-04 12:21:31', 'Login to System'),
(58, 1, '2024-07-04 12:23:52', 'Login to System'),
(59, 1, '2024-07-04 14:55:51', 'Login to System'),
(60, 1, '2024-07-04 15:07:13', 'Login to System'),
(61, 1, '2024-07-04 15:11:08', 'Login to System'),
(62, 1, '2024-07-04 15:15:30', 'Login to System'),
(63, 1, '2024-07-06 14:19:38', 'Login to System'),
(64, 1, '2024-07-08 11:14:48', 'Login to System'),
(65, 1, '2024-07-08 12:23:24', 'Login to System'),
(66, 1, '2024-07-08 12:30:27', 'Login to System'),
(67, 2, '2024-07-09 14:07:28', 'Login to System'),
(68, 2, '2024-07-09 14:25:10', 'Login to System'),
(69, 2, '2024-07-09 14:31:03', 'Login to System'),
(70, 2, '2024-07-09 14:39:30', 'Login to System'),
(71, 2, '2024-07-09 14:41:46', 'Login to System'),
(72, 2, '2024-07-09 14:43:54', 'Login to System'),
(73, 2, '2024-07-09 14:46:54', 'Login to System'),
(74, 2, '2024-07-09 14:49:22', 'Login to System'),
(75, 2, '2024-07-09 14:59:12', 'Login to System'),
(76, 2, '2024-07-13 00:15:14', 'Login to System'),
(77, 2, '2024-07-13 13:26:19', 'Login to System'),
(78, 2, '2024-07-13 13:34:38', 'Login to System'),
(79, 2, '2024-07-13 13:37:59', 'Login to System'),
(80, 2, '2024-07-13 13:39:07', 'Login to System'),
(81, 2, '2024-07-13 13:57:15', 'Login to System'),
(82, 2, '2024-07-13 14:01:59', 'Login to System'),
(83, 2, '2024-07-13 14:22:00', 'Login to System'),
(84, 2, '2024-07-13 14:40:47', 'Login to System'),
(85, 2, '2024-07-13 15:37:32', 'Login to System'),
(86, 2, '2024-07-13 15:38:11', 'Login to System'),
(87, 2, '2024-07-13 15:41:13', 'Login to System'),
(88, 2, '2024-07-13 15:42:15', 'Login to System'),
(89, 2, '2024-07-13 15:54:10', 'Login to System'),
(90, 2, '2024-07-13 15:56:53', 'Login to System'),
(91, 2, '2024-07-13 16:00:14', 'Login to System'),
(92, 2, '2024-07-13 16:01:06', 'Login to System'),
(93, 2, '2024-07-13 16:03:37', 'Login to System'),
(94, 2, '2024-07-13 16:04:10', 'Login to System'),
(95, 2, '2024-07-13 16:04:54', 'Login to System'),
(96, 2, '2024-07-13 16:05:44', 'Login to System'),
(97, 2, '2024-07-13 16:06:39', 'Login to System'),
(98, 2, '2024-07-13 21:14:22', 'Login to System'),
(99, 2, '2024-07-13 21:15:30', 'Login to System'),
(100, 2, '2024-07-13 21:17:18', 'Login to System'),
(101, 2, '2024-07-13 21:20:48', 'Login to System'),
(102, 2, '2024-07-13 21:28:22', 'Login to System'),
(103, 2, '2024-07-13 21:39:55', 'Login to System'),
(104, 2, '2024-07-13 21:45:25', 'Login to System'),
(105, 2, '2024-07-13 21:52:23', 'Login to System'),
(106, 2, '2024-07-13 21:53:12', 'Login to System'),
(107, 2, '2024-07-13 21:54:29', 'Login to System'),
(108, 2, '2024-07-13 21:56:11', 'Login to System'),
(109, 2, '2024-07-13 22:10:33', 'Login to System'),
(110, 2, '2024-07-13 22:14:25', 'Login to System'),
(111, 2, '2024-07-13 22:16:12', 'Login to System'),
(112, 2, '2024-07-13 22:18:55', 'Login to System'),
(113, 2, '2024-07-13 22:21:09', 'Login to System'),
(114, 2, '2024-07-13 22:25:25', 'Login to System'),
(115, 2, '2024-07-13 22:26:55', 'Login to System'),
(116, 2, '2024-07-13 22:29:51', 'Login to System'),
(117, 2, '2024-07-13 22:31:09', 'Login to System'),
(118, 2, '2024-07-13 22:32:05', 'Login to System'),
(119, 2, '2024-07-13 22:32:39', 'Login to System'),
(120, 2, '2024-07-13 22:34:13', 'Login to System'),
(121, 2, '2024-07-13 22:43:20', 'Login to System'),
(122, 2, '2024-07-13 22:50:52', 'Login to System'),
(123, 2, '2024-07-13 23:06:18', 'Login to System'),
(124, 2, '2024-07-13 23:08:04', 'Login to System'),
(125, 2, '2024-07-13 23:08:59', 'Login to System'),
(126, 2, '2024-07-13 23:09:52', 'Login to System'),
(127, 2, '2024-07-13 23:15:34', 'Login to System'),
(128, 2, '2024-07-14 10:43:17', 'Login to System'),
(129, 2, '2024-07-14 10:46:07', 'Login to System'),
(130, 2, '2024-07-14 11:17:15', 'Login to System'),
(131, 2, '2024-07-14 11:20:27', 'Login to System'),
(132, 2, '2024-07-14 11:20:51', 'Login to System'),
(133, 2, '2024-07-14 11:22:06', 'Login to System'),
(134, 2, '2024-07-14 11:24:05', 'Login to System'),
(135, 2, '2024-07-14 11:27:09', 'Login to System'),
(136, 2, '2024-07-14 11:33:36', 'Login to System'),
(137, 2, '2024-07-14 11:35:58', 'Login to System'),
(138, 2, '2024-07-14 11:44:47', 'Login to System'),
(139, 2, '2024-07-14 14:55:53', 'Login to System'),
(140, 2, '2024-07-14 15:11:41', 'Login to System'),
(141, 2, '2024-07-14 15:23:43', 'Login to System'),
(142, 2, '2024-07-14 15:27:23', 'Login to System'),
(143, 2, '2024-07-14 15:29:50', 'Login to System'),
(144, 2, '2024-07-14 15:31:42', 'Login to System'),
(145, 2, '2024-07-14 15:32:23', 'Login to System'),
(146, 2, '2024-07-14 15:36:07', 'Login to System'),
(147, 2, '2024-07-14 15:41:49', 'Login to System'),
(148, 2, '2024-07-14 15:43:43', 'Login to System'),
(149, 2, '2024-07-14 20:10:32', 'Login to System'),
(150, 2, '2024-07-14 20:11:17', 'Login to System'),
(151, 2, '2024-07-14 20:12:10', 'Login to System'),
(152, 2, '2024-07-14 20:19:39', 'Login to System'),
(153, 2, '2024-07-14 20:23:15', 'Login to System'),
(154, 2, '2024-07-14 21:30:00', 'Login to System'),
(155, 2, '2024-07-14 21:49:57', 'Login to System'),
(156, 2, '2024-07-14 21:52:45', 'Login to System'),
(157, 2, '2024-07-14 23:06:06', 'Login to System'),
(158, 2, '2024-07-14 23:16:47', 'Login to System'),
(159, 2, '2024-07-14 23:18:52', 'Login to System'),
(160, 2, '2024-07-14 23:29:30', 'Login to System'),
(161, 2, '2024-07-14 23:32:41', 'Login to System'),
(162, 2, '2024-07-14 23:33:22', 'Login to System'),
(163, 2, '2024-07-15 22:50:13', 'Login to System'),
(164, 2, '2024-07-15 22:54:04', 'Login to System'),
(165, 2, '2024-07-15 22:55:43', 'Login to System'),
(166, 2, '2024-07-15 22:58:05', 'Login to System'),
(167, 2, '2024-07-15 23:00:17', 'Login to System'),
(168, 2, '2024-07-15 23:11:11', 'Login to System'),
(169, 2, '2024-07-15 23:27:07', 'Login to System'),
(170, 2, '2024-07-15 23:32:58', 'Login to System'),
(171, 2, '2024-07-15 23:47:37', 'Login to System'),
(172, 2, '2024-07-16 10:25:24', 'Login to System'),
(173, 2, '2024-07-16 11:07:29', 'Login to System'),
(174, 2, '2024-07-16 11:11:55', 'Login to System'),
(175, 2, '2024-07-16 11:14:53', 'Login to System'),
(176, 2, '2024-07-16 11:17:11', 'Login to System'),
(177, 2, '2024-07-16 11:23:11', 'Login to System'),
(178, 2, '2024-07-16 11:28:19', 'Login to System'),
(179, 2, '2024-07-16 11:31:36', 'Login to System'),
(180, 2, '2024-07-16 11:35:41', 'Login to System'),
(181, 2, '2024-07-16 11:40:29', 'Login to System'),
(182, 2, '2024-07-16 11:43:12', 'Login to System'),
(183, 2, '2024-07-16 20:28:27', 'Login to System'),
(184, 2, '2024-07-16 20:42:38', 'Login to System'),
(185, 2, '2024-07-16 20:49:58', 'Login to System'),
(186, 2, '2024-07-16 20:54:14', 'Login to System'),
(187, 2, '2024-07-16 20:57:28', 'Login to System'),
(188, 2, '2024-07-16 21:03:05', 'Login to System'),
(189, 2, '2024-07-16 21:08:32', 'Login to System'),
(190, 2, '2024-07-16 21:13:00', 'Login to System'),
(191, 2, '2024-07-16 21:15:51', 'Login to System'),
(192, 2, '2024-07-16 21:18:03', 'Login to System'),
(193, 2, '2024-07-16 21:26:20', 'Login to System'),
(194, 2, '2024-07-16 21:37:54', 'Login to System'),
(195, 2, '2024-07-16 21:41:52', 'Login to System'),
(196, 2, '2024-07-16 21:44:58', 'Login to System'),
(197, 2, '2024-07-16 21:46:55', 'Login to System'),
(198, 2, '2024-07-16 21:50:34', 'Login to System'),
(199, 2, '2024-07-16 21:56:08', 'Login to System'),
(200, 2, '2024-07-16 22:08:10', 'Login to System'),
(201, 2, '2024-07-16 22:10:51', 'Login to System'),
(202, 2, '2024-07-16 22:12:29', 'Login to System'),
(203, 2, '2024-07-16 22:15:25', 'Login to System'),
(204, 2, '2024-07-16 22:18:54', 'Login to System'),
(205, 2, '2024-07-16 22:21:31', 'Login to System'),
(206, 2, '2024-07-16 22:25:48', 'Login to System'),
(207, 2, '2024-07-16 22:35:28', 'Login to System'),
(208, 2, '2024-07-16 22:59:08', 'Login to System'),
(209, 2, '2024-07-16 23:04:48', 'Login to System'),
(210, 2, '2024-07-16 23:16:28', 'Login to System'),
(211, 2, '2024-07-16 23:20:22', 'Login to System'),
(212, 2, '2024-07-16 23:23:08', 'Login to System'),
(213, 2, '2024-07-16 23:59:47', 'Login to System'),
(214, 2, '2024-07-17 10:32:45', 'Login to System'),
(215, 2, '2024-07-17 10:42:57', 'Login to System'),
(216, 2, '2024-07-17 11:21:21', 'Login to System'),
(217, 2, '2024-07-17 11:46:06', 'Login to System'),
(218, 2, '2024-07-17 11:49:20', 'Login to System'),
(219, 2, '2024-07-17 11:52:31', 'Login to System'),
(220, 2, '2024-07-17 11:56:44', 'Login to System'),
(221, 2, '2024-07-17 11:59:18', 'Login to System'),
(222, 2, '2024-07-17 12:01:10', 'Login to System'),
(223, 2, '2024-07-17 12:03:45', 'Login to System'),
(224, 2, '2024-07-17 12:29:13', 'Login to System'),
(225, 2, '2024-07-17 12:32:06', 'Login to System'),
(226, 2, '2024-07-17 12:33:11', 'Login to System'),
(227, 1, '2024-07-17 14:09:48', 'Login to System'),
(228, 1, '2024-07-17 16:52:52', 'Login to System'),
(229, 1, '2024-07-17 16:58:37', 'Login to System'),
(230, 1, '2024-07-17 17:01:26', 'Login to System'),
(231, 1, '2024-07-17 17:14:41', 'Login to System'),
(232, 1, '2024-07-17 17:17:57', 'Login to System'),
(233, 2, '2024-07-18 14:27:05', 'Login to System'),
(234, 2, '2024-07-18 15:19:51', 'Login to System'),
(235, 2, '2024-07-18 15:30:05', 'Login to System'),
(236, 2, '2024-07-18 15:38:56', 'Login to System'),
(237, 1, '2024-07-20 20:02:12', 'Login to System'),
(238, 1, '2024-07-20 20:18:18', 'Login to System'),
(239, 1, '2024-07-20 21:51:24', 'Login to System'),
(240, 1, '2024-07-20 22:03:40', 'Login to System'),
(241, 1, '2024-07-20 22:14:30', 'Login to System'),
(242, 1, '2024-07-21 15:27:13', 'Login to System'),
(243, 1, '2024-07-21 15:28:00', 'Login to System'),
(244, 1, '2024-07-21 16:12:33', 'Login to System'),
(245, 1, '2024-07-21 16:13:35', 'Login to System'),
(246, 1, '2024-07-21 16:27:09', 'Login to System'),
(247, 1, '2024-07-21 16:36:16', 'Login to System'),
(248, 2, '2024-07-21 21:54:54', 'Login to System'),
(249, 2, '2024-07-21 22:48:56', 'Login to System'),
(250, 2, '2024-07-21 22:50:36', 'Login to System'),
(251, 2, '2024-07-21 22:52:05', 'Login to System'),
(252, 2, '2024-07-21 22:56:52', 'Login to System'),
(253, 2, '2024-07-21 23:04:23', 'Login to System'),
(254, 2, '2024-07-21 23:12:43', 'Login to System'),
(255, 2, '2024-07-21 23:22:13', 'Login to System'),
(256, 2, '2024-07-21 23:24:28', 'Login to System'),
(257, 2, '2024-07-22 20:07:47', 'Login to System'),
(258, 2, '2024-07-22 20:15:39', 'Login to System'),
(259, 2, '2024-07-22 21:24:04', 'Login to System'),
(260, 2, '2024-07-22 21:54:11', 'Login to System'),
(261, 2, '2024-07-23 10:39:07', 'Login to System'),
(262, 2, '2024-07-23 10:41:15', 'Login to System'),
(263, 2, '2024-07-23 11:09:43', 'Login to System'),
(264, 2, '2024-07-23 11:23:42', 'Login to System'),
(265, 2, '2024-07-23 11:25:34', 'Login to System'),
(266, 2, '2024-07-23 11:48:04', 'Login to System'),
(267, 2, '2024-07-23 11:56:56', 'Login to System'),
(268, 2, '2024-07-23 12:05:40', 'Login to System'),
(269, 2, '2024-07-23 12:06:35', 'Login to System'),
(270, 2, '2024-07-23 13:23:30', 'Login to System'),
(271, 2, '2024-07-23 13:33:23', 'Login to System'),
(272, 2, '2024-07-23 13:35:02', 'Login to System'),
(273, 2, '2024-07-23 13:38:34', 'Login to System'),
(274, 2, '2024-07-23 15:01:19', 'Login to System'),
(275, 2, '2024-07-23 15:21:35', 'Login to System'),
(276, 2, '2024-07-23 15:24:33', 'Login to System'),
(277, 2, '2024-07-23 15:32:12', 'Login to System'),
(278, 2, '2024-07-23 15:44:57', 'Login to System'),
(279, 2, '2024-07-23 15:54:19', 'Login to System'),
(280, 2, '2024-07-23 15:56:34', 'Login to System'),
(281, 2, '2024-07-23 16:04:11', 'Login to System'),
(282, 1, '2024-07-23 20:44:44', 'Login to System'),
(283, 1, '2024-07-23 20:44:44', 'Login to System'),
(284, 2, '2024-07-23 21:15:51', 'Login to System'),
(285, 1, '2024-07-23 22:00:47', 'Login to System'),
(286, 1, '2024-07-23 22:18:18', 'Login to System'),
(287, 2, '2024-07-23 22:24:58', 'Login to System'),
(288, 2, '2024-07-23 22:57:13', 'Login to System'),
(289, 2, '2024-07-23 23:29:12', 'Login to System'),
(290, 2, '2024-07-23 23:40:58', 'Login to System'),
(291, 2, '2024-07-24 05:55:20', 'Login to System'),
(292, 2, '2024-07-24 06:00:19', 'Login to System'),
(293, 2, '2024-07-24 06:08:58', 'Login to System'),
(294, 2, '2024-07-24 06:23:01', 'Login to System'),
(295, 2, '2024-07-24 06:49:02', 'Login to System'),
(296, 2, '2024-07-24 07:17:37', 'Login to System'),
(297, 1, '2024-07-24 08:00:04', 'Login to System'),
(298, 1, '2024-07-24 08:13:06', 'Login to System'),
(299, 1, '2024-07-24 08:16:10', 'Login to System'),
(300, 1, '2024-07-24 08:56:45', 'Login to System'),
(301, 1, '2024-07-24 09:01:24', 'Login to System'),
(302, 19, '2024-07-24 10:26:57', 'Login to System'),
(303, 2, '2024-07-24 10:28:13', 'Login to System'),
(304, 2, '2024-07-24 10:29:02', 'Login to System'),
(305, 2, '2024-07-24 10:36:53', 'Login to System'),
(306, 2, '2024-07-24 10:40:45', 'Login to System'),
(307, 2, '2024-07-24 10:46:07', 'Login to System'),
(308, 19, '2024-07-24 10:58:03', 'Login to System'),
(309, 2, '2024-07-24 11:00:17', 'Login to System'),
(310, 2, '2024-07-24 11:00:57', 'Login to System'),
(311, 2, '2024-07-24 11:12:30', 'Login to System'),
(312, 1, '2024-07-24 11:26:06', 'Login to System'),
(313, 1, '2024-07-24 11:35:04', 'Generate Activation Code'),
(314, 1, '2024-07-24 11:35:08', 'Generate Activation Code'),
(315, 2, '2024-07-24 14:21:07', 'Login to System'),
(316, 2, '2024-07-24 14:28:54', 'Login to System'),
(317, 2, '2024-07-24 22:39:54', 'Login to System'),
(318, 1, '2024-07-24 22:40:42', 'Login to System'),
(319, 1, '2024-07-24 22:41:28', 'Generate Activation Code'),
(320, 1, '2024-07-24 23:30:54', 'Login to System'),
(321, 1, '2024-07-24 23:38:40', 'Login to System'),
(322, 2, '2024-07-26 13:28:23', 'Login to System'),
(323, 1, '2024-07-26 13:30:51', 'Login to System'),
(324, 2, '2024-07-26 13:38:24', 'Login to System'),
(325, 1, '2024-07-26 13:39:22', 'Login to System'),
(326, 2, '2024-07-26 13:50:15', 'Login to System'),
(327, 2, '2024-07-26 13:52:42', 'Login to System'),
(328, 2, '2024-07-26 13:55:54', 'Login to System'),
(329, 2, '2024-07-26 14:56:57', 'Login to System'),
(330, 1, '2024-07-26 15:00:08', 'Login to System'),
(331, 1, '2024-07-26 15:00:27', 'Generate Activation Code'),
(332, 1, '2024-07-26 15:00:30', 'Generate Activation Code'),
(333, 1, '2024-07-26 15:00:57', 'Login to System'),
(334, 2, '2024-08-15 19:44:34', 'Login to System'),
(335, 2, '2024-08-15 19:52:09', 'Login to System'),
(336, 2, '2024-08-15 20:39:32', 'Login to System'),
(337, 2, '2024-08-17 21:25:08', 'Login to System'),
(338, 2, '2024-08-17 21:56:11', 'Login to System'),
(339, 1, '2024-08-17 22:08:05', 'Login to System'),
(340, 1, '2024-08-17 22:11:43', 'Generate Activation Code'),
(341, 2, '2024-08-17 22:13:32', 'Login to System');

create table Staff(
	idStaff int primary key auto_increment,
    Name varchar(255),
    Salary double,
    IdUser int,
    IsDeleted boolean,
    
    foreign key (IdUser) references User(idUser)
);
INSERT INTO Staff (Name, Salary, IdUser, IsDeleted) 
VALUES
('Thao Nguyen', 1500.00, 1, 0),
('Tri Cuong', 2000.00, 3, 0),
('Tom Lee', 1800.00, 4, 0),
('Cong Cao', 2100.00, 5, 0),
('Trong Nhan', 1750.00, 6, 0);


create table Customer(
	idCustomer int primary key auto_increment,
    Name varchar(255),
    Image text,
    IdUser int, 	
    IsDeleted boolean,
    
    foreign key (IdUser) references User(idUser)
);
INSERT INTO Customer (Name, Image, IdUser, IsDeleted) 
VALUES
('Nguyen Van A', 'D:\\HOCTAP\\WPF\\QLMaKichHoat\\QuanLyMaWinApp\\QuanLyMaWinApp\\Resources\\Images\\dotnet_bot.png', 2, 0),
('Tran Thi B', 'D:\\HOCTAP\\WPF\\QLMaKichHoat\\QuanLyMaWinApp\\QuanLyMaWinApp\\Resources\\Images\\dotnet_bot.png', 3, 0),
('Le Van C', 'D:\\HOCTAP\\WPF\\QLMaKichHoat\\QuanLyMaWinApp\\QuanLyMaWinApp\\Resources\\Images\\dotnet_bot.png', 4, 0),
('Pham Thi D', 'D:\\HOCTAP\\WPF\\QLMaKichHoat\\QuanLyMaWinApp\\QuanLyMaWinApp\\Resources\\Images\\dotnet_bot.png', 5, 0),
('Hoang Van E', 'D:\\HOCTAP\\WPF\\QLMaKichHoat\\QuanLyMaWinApp\\QuanLyMaWinApp\\Resources\\Images\\dotnet_bot.png', 6, 0);

create table Orders(
	ID int primary key auto_increment,
    IdUser int, 
    DCGH text,
    NgayDat date, 
    NgayGiao date,
    SLTong int,
    TongTien double,
    ThanhToanStatus enum ('Napas','Credit'),
    DonHangStatus enum('Processing','Delivered','Cancelled','Confirmed'),
    
    foreign key (IdUser) references User(idUser)
);

ALTER TABLE Orders
MODIFY COLUMN TongTien DOUBLE DEFAULT 0;

ALTER TABLE orders
MODIFY COLUMN NgayGiao DATE DEFAULT NULL;  -- Nếu bạn muốn đổi thành kiểu DATE

DELIMITER //

CREATE TRIGGER UpdateNgayGiaoOnConfirmed
BEFORE UPDATE ON Orders
FOR EACH ROW
BEGIN
    IF NEW.DonHangStatus = 'Confirmed' THEN
        SET NEW.NgayGiao = DATE_ADD(NEW.NgayDat, INTERVAL 7 DAY);
    END IF;
END;

//

DELIMITER ;


DELIMITER //

CREATE TRIGGER CheckNgayGiaoAfterInsert
AFTER INSERT ON Orders
FOR EACH ROW
BEGIN
    IF NEW.NgayGiao <= NEW.NgayDat THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Error: NgayGiao must be greater than NgayDat after insert.';
    END IF;
END; //

CREATE TRIGGER CheckNgayGiaoAfterUpdate
AFTER UPDATE ON Orders
FOR EACH ROW
BEGIN
    IF NEW.NgayGiao <= NEW.NgayDat THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Error: NgayGiao must be greater than NgayDat after update.';
    END IF;
END; //

DELIMITER ;

ALTER TABLE `orders`
ADD COLUMN note TEXT DEFAULT NULL;

INSERT INTO `orders` (`ID`, `IdUser`, `DCGH`, `NgayDat`, `NgayGiao`, `ThanhToanStatus`, `DonHangStatus`, `SLTong`, `TongTien`) VALUES
(38, 2, '111 ddd', '2023-07-16', '2023-07-18', 'Napas', 'Delivered', 1, 280.49),
(39, 2, '123 abc', '2024-07-17', '2024-07-24', 'Credit', 'Confirmed', 8, 7549.65),
(40, 2, '115 le trong tan', '2024-07-18', '2024-07-25', 'Credit', 'Delivered', 3, 1186.33),
(41, 2, '140 le trong tan', '2024-07-18', '2024-07-25', 'Credit', 'Confirmed', 1, 883.60),
(42, 2, '140 Le Trong Tan', '2024-07-24', '2024-07-31', 'Napas', 'Confirmed', 3, 958.75),
(43, 2, '140 Le Trong Tan', '2024-07-24', '2024-07-31', 'Napas', 'Confirmed', 3, 841.47),
(44, 2, '140 Le Trong Tan', '2024-07-26', '2024-08-02', 'Credit', 'Confirmed', 3, 1686.56),
(45, 2, '140 Le Trong Tan', '2024-07-26', '2024-08-02', 'Napas', 'Confirmed', 1, 359.55),
(46, 2, '140 Le Trong Tan', '2024-07-26', '2024-08-02', 'Credit', 'Confirmed', 1, 982.44),
(47, 2, '140 Le Trong Tan', '2024-07-26', '2024-08-02', 'Credit', 'Cancelled', 1, 359.55),
(48, 2, '140 Le Trong Tan', '2024-07-26', '2024-08-02', 'Napas', 'Confirmed', 2, 719.10),
(49, 2, '140 Le Trong Tan', '2024-07-26', '2024-08-02', 'Credit', 'Cancelled', 1, 280.49),
(50, 2, '140 Le Trong Tan', '2024-08-15', '2024-08-22', 'Napas', 'Confirmed', 5, 1572.29),
(51, 2, '140 Le Trong Tan', '2024-08-15', '2024-08-22', 'Credit', 'Processing', 1, 165.65),
(52, 2, '140 Le Trong Tan', '2024-08-17', '2024-08-24', 'Napas', 'Confirmed', 1, 186.86),
(53, 2, '140 Le Trong Tan', '2024-08-17', '2024-08-24', 'Napas', 'Confirmed', 1, 165.65),
(54, 2, '140 Le Trong Tan', '2024-08-17', '2024-08-24', 'Credit', 'Processing', 1, 359.55);

create table Receipt(
	IdOrder int,
    IdStaff int,
	IdShipper int,
    ShippingDate date,
    Note text,
    
    foreign key (IdOrder) references Orders(ID),
    foreign key (IdStaff) references Staff(idStaff),
    foreign key (IdShipper) references Staff(idStaff)
);
INSERT INTO Receipt (IdOrder, IdStaff, IdShipper, ShippingDate, Note) 
VALUES
(38, 1, 2, '2024-10-01', 'Delivered successfully'),
(52, 3, 4, '2024-10-02', 'Delayed due to weather conditions'),
(39, 2, 5, '2024-10-03', 'Customer requested rescheduling'),
(40, 4, 1, '2024-10-04', 'Delivered, no issues'),
(51, 5, 3, '2024-10-05', 'Address not found, awaiting confirmation');

create table Nhacungcap(
	idNCC int primary key auto_increment,
    TenNCC varchar(255),
    Diachi text,
    Email varchar(255),
    Phone varchar (25)
);

INSERT INTO Nhacungcap (TenNCC, Diachi, Email, Phone) 
VALUES
('Công ty A', '123 Đường ABC, Quận 1, TP.HCM', 'congtya@example.com', '0909123456'),
('Công ty B', '456 Đường DEF, Quận 2, TP.HCM', 'congtyb@example.com', '0909345678'),
('Công ty C', '789 Đường GHI, Quận 3, TP.HCM', 'congtyc@example.com', '0912345678'),
('Công ty D', '101 Đường JKL, Quận 4, TP.HCM', 'congtyd@example.com', '0913456789'),
('Công ty E', '202 Đường MNO, Quận 5, TP.HCM', 'congtye@example.com', '0923456789');


create table Phieunhap(
	idPN int primary key auto_increment,
    idNCC int, 
    NgayNhap date, 
    IsDeleted boolean,
    
    foreign key (idNCC) references Nhacungcap(idNCC)
);
INSERT INTO Phieunhap (idNCC, NgayNhap, IsDeleted) 
VALUES
(1, '2024-10-01', 0),
(2, '2024-10-02', 0),
(3, '2024-10-03', 0),
(4, '2024-10-04', 0),
(5, '2024-10-05', 0);

create table Category(
	ID varchar(255) primary key,
    CategoryName varchar(255),
	Detail text,
	IsDeleted boolean
);

ALTER TABLE `category`
ALTER IsDeleted SET DEFAULT 0;

INSERT INTO `category` ( `CategoryName`, `Detail`, `IsDeleted`) VALUES
( 'NMR Spectroscopy 1', 'NMR machines for molecular structure and dynamics analysis.', 1),
( 'Mass Spectrometry1', 'Mass spectrometry equipment for chemical and biological research.', 0),
( 'X-Ray Diffraction', 'X-ray diffraction systems for crystal structure research.', 0),
( 'Atomic Force Microscopy', 'AFM microscope for material surface analysis.', 0),
( 'Confocal Microscopy', 'Confocal microscope for observing biological samples and materials.', 1),
( 'FT-IR Spectroscopy', 'FT-IR machines for chemical analysis and material quality.', 1),
( 'HPLC Chromatography', 'HPLC machines for high-quality analysis in research and industry.', 0),
( 'MRI Imaging', 'MRI systems for biological and animal imaging research.', 0),
( 'Chemical Analysis', 'Chemical and elemental analysis equipment.', 0),
( 'Bioinformatics Solutions', 'Software solutions for protein analysis and genomics.', 0),
( 'Bioinformatics Solutions 1', 'Software solutions for protein analysis and genomi 1.', 0),
( 'Bioinformatics Solutions 2', 'Software solutions for protein analysis and genomi 2.', 0),
( 'Bioinformatics Solutions ', 'Software solutions for protein analysis and genomi 3.', 0);


create table ProductVersion (
	ID int primary key auto_increment,
    Version varchar(255),
    Description text,
    Price double
);
INSERT INTO ProductVersion (Version, Description, Price) 
VALUES
('1.0', 'Phiên bản đầu tiên của sản phẩm', 100.00),
('1.1', 'Cập nhật nhỏ với một số sửa lỗi', 120.00),
('2.0', 'Phiên bản lớn với nhiều tính năng mới', 150.00),
('2.1', 'Cải thiện hiệu suất và sửa lỗi', 160.00),
('3.0', 'Phiên bản hoàn toàn mới với giao diện người dùng cải tiến', 200.00);


create table ActivationCode(
	ID int primary key auto_increment,
    Acticode varchar(255),
    Status enum('Active','Expired'),
    NgayKhoiTao date,
    NgayHetHan date,
    DinhKy 	ENUM('6','12','24'),
    Price double  
);

ALTER TABLE ActivationCode
MODIFY COLUMN Status ENUM('Active', 'Expired') DEFAULT 'Active';

DELIMITER //

CREATE TRIGGER UpdatePriceBeforeInsert
BEFORE INSERT ON ActivationCode
FOR EACH ROW
BEGIN
    IF NEW.DinhKy = '6' THEN
        SET NEW.Price = 89.99;
        SET NEW.NgayHetHan = DATE_ADD(NEW.NgayKhoiTao, INTERVAL 6 MONTH);
    ELSEIF NEW.DinhKy = '12' THEN
        SET NEW.Price = 189.1;
        SET NEW.NgayHetHan = DATE_ADD(NEW.NgayKhoiTao, INTERVAL 1 YEAR);
    ELSEIF NEW.DinhKy = '24' THEN
        SET NEW.Price = 388.85;
        SET NEW.NgayHetHan = DATE_ADD(NEW.NgayKhoiTao, INTERVAL 2 YEAR);
    END IF;
END;

//

DELIMITER ;

DELIMITER //

CREATE TRIGGER UpdatePriceBeforeUpdate
BEFORE UPDATE ON ActivationCode
FOR EACH ROW
BEGIN
    IF NEW.DinhKy = '6' THEN
        SET NEW.Price = 89.99;
        SET NEW.NgayHetHan = DATE_ADD(NEW.NgayKhoiTao, INTERVAL 6 MONTH);
    ELSEIF NEW.DinhKy = '12' THEN
        SET NEW.Price = 189.1;
        SET NEW.NgayHetHan = DATE_ADD(NEW.NgayKhoiTao, INTERVAL 1 YEAR);
    ELSEIF NEW.DinhKy = '24' THEN
        SET NEW.Price = 388.85;
        SET NEW.NgayHetHan = DATE_ADD(NEW.NgayKhoiTao, INTERVAL 2 YEAR);
    END IF;
END;

//

DELIMITER ;


INSERT INTO `activationcode` (`ID`, `ActiCode`, `Status`, `NgayKhoiTao`, `NgayHetHan`, `DinhKy`, `Price`) VALUES
(19, '96862a4c5dfe', 'Expired', '2023-07-16 00:00:00', '2024-07-16 00:00:00', '6', 89.99),
(20, 'bcb750c7b124', 'Active', '2024-07-17 00:00:00', '2025-07-17 00:00:00', '6', 89.99),
(21, '4e99585a890f', 'Active', '2024-07-18 00:00:00', '2025-07-18 00:00:00', '6', 89.99),
(22, '546583c855db', 'Active', '2024-07-18 00:00:00', '2025-07-18 00:00:00', '6', 89.99),
(23, 'a51e4d8225bc', 'Active', '2024-07-24 00:00:00', '2025-07-24 00:00:00', '6', 89.99),
(24, '2be4ac1f944c', 'Active', '2024-07-24 00:00:00', '2025-07-24 00:00:00',  '12', 89.99),
(25, 'da5695b56bd6', 'Active', '2024-07-24 00:00:00', '2027-07-24 00:00:00', '12', 267.90),
(26, 'cad31ba4ae89', 'Expired', '2020-07-24 00:00:00', '2023-07-24 00:00:00', '12', 267.90),
(27, 'a8cebc2f16e3', 'Active', '2024-07-24 00:00:00', '2025-07-24 00:00:00',  '24', 89.99),
(28, 'd92f57e7585f', 'Expired', '2016-07-24 00:00:00', '2024-07-24 00:00:00',  '24', 719.00),
(29, 'a7363fa0822e', 'Active', '2024-07-26 00:00:00', '2025-07-26 00:00:00',  '24', 89.99),
(30, 'f7ada46645b4', 'Active', '2024-07-26 00:00:00', '2025-07-26 00:00:00',  '12', 89.99),
(31, 'c1919c3efa39', 'Active', '2024-07-26 00:00:00', '2025-07-26 00:00:00',  '12', 89.99),
(32, '8578d37fca6f', 'Active', '2024-07-26 00:00:00', '2025-07-26 00:00:00',  '12', 89.99),
(33, 'bdb601bfb847', 'Active', '2024-07-26 00:00:00', '2025-07-26 00:00:00',  '12', 89.99),
(34, 'c92bd21ae572', 'Active', '2024-07-26 00:00:00', '2025-07-26 00:00:00',  '12', 89.99),
(35, '1cca9b1035f9', 'Active', '2024-07-26 00:00:00', '2025-07-26 00:00:00',  '12', 89.99),
(36, 'a7e2fe5200d2', 'Expired', '2024-07-26 00:00:00', '2025-07-26 00:00:00',  '12', 89.99),
(37, '056504bb4ed8', 'Active', '2024-08-15 00:00:00', '2025-08-15 00:00:00',  '6', 89.99),
(38, '2daddd902930', 'Active', '2024-08-15 00:00:00', '2025-08-15 00:00:00',  '6', 89.99),
(39, 'f7e9bcc7c8cc', 'Active', '2024-08-17 00:00:00', '2025-08-17 00:00:00',  '6', 89.99),
(40, 'b063499f2ed9', 'Active', '2024-08-17 00:00:00', '2025-08-17 00:00:00',  '6', 89.99),
(41, '41e40ce3dea5', 'Active', '2024-08-17 00:00:00', '2025-08-17 00:00:00', '6', 89.99),
(42, '603a859ec4fb', 'Active', '2024-08-17 00:00:00', '2025-08-17 00:00:00',  '6', 89.99);


create table Product(
	ID VARCHAR(255) primary key NOT NULL,
    Name varchar(255),
    IdCate varchar(255),
    Evaluate DOUBLE,
    SL int,
    Price double,
    Detail text,
    Feature	text,
    Specifications text,
    Helps text,
    IdVersion int,
    IsDeleted boolean NOT NULL,
    image text,
    
    foreign key (IdCate) references Category(ID),
    foreign key (IdVersion) references ProductVersion(ID)
);

ALTER TABLE `Product`
ALTER IsDeleted SET DEFAULT 0;

INSERT INTO Product (ID, Name, IdCate, Evaluate, SL, Price, Detail, Feature, Specifications, Helps, IdVersion, IsDeleted, image) VALUES 
('1','Ascend 400 MHz NMR', 8, 4.8, 0, 190.50, 'Advanced NMR spectrometer for molecular structure analysis.', 'High Resolution', '400 MHz', 'User-friendly', NULL, 0, product1.png),
('2','Avance III HD', 1, 4.7, 1, 75.66, 'High-performance NMR system for advanced research.', 'High Sensitivity', 'Ultra High Field', 'Versatile Applications', NULL, 0, product1.png),
('3','DRX-400', 1, 4.5, 4, 96.87, 'NMR system for both routine and research applications.', 'Versatile Instrument', '400 MHz', 'Robust Performance', NULL, 0, product1.png),
('4','ultrafleXtreme MALDI-TOF', 2, 4.9, -4, 269.56, 'High-resolution MALDI-TOF mass spectrometer for proteomics.', 'High Resolution', 'MALDI-TOF', 'Advanced Proteomics', NULL, 0, product1.png),
('5','Autoflex Speed', 2, 4.6, 3, 306.56, 'Fast and reliable MALDI-TOF mass spectrometer for chemical analysis.', 'High Sensitivity', 'Rapid Analysis', 'Chemical Research', NULL, 0, product1.png),
('6','D8 QUEST X-Ray Diffraction', 3, 4.8, 2, 910.65, 'High-end X-ray diffraction system for crystal structure determination.', 'High Resolution', 'X-Ray Diffraction', 'Accurate Analysis', NULL, 0, product1.png),
('7','D8 ADVANCE X-Ray Diffraction', 3, 4.7, 3, 1569.96, 'X-ray diffraction system for various material research.', 'Versatile Instrument', 'High Performance', 'Material Analysis', NULL, 0, product1.png),
('8','Dimension Icon AFM', 4, 4.9, 4, 963.54, 'Advanced AFM for surface characterization at the atomic level.', 'Atomic Resolution', 'AFM', 'Surface Analysis', NULL, 0, product1.png),
('9','LMD7 Laser Microdissection', 5, 4.8, 178, 793.61, 'Laser microdissection system for biological research.', 'High Precision', 'Microdissection', 'Biological Research', NULL, 0, product1.png),
('10','Alpha II FT-IR Spectrometer', 6, 4.6, 46, 196.36, 'FT-IR spectrometer for a wide range of chemical analysis applications.', 'Reliable Performance', 'FT-IR', 'Chemical Analysis', NULL, 0, product1.png),
('11','Vertex 80 FT-IR Spectrometer', 6, 4.7, 3, 984.75, 'High-end FT-IR spectrometer for advanced research.', 'High Sensitivity', 'FT-IR', 'Advanced Applications', NULL, 0, product1.png),
('12','Nexera X2 HPLC System', 7, 4.8, 6, 769.45, 'Advanced HPLC system for high-performance liquid chromatography.', 'High Throughput', 'HPLC', 'Chemical Analysis', NULL, 0, product1.png),
('13','InfinityLab LC Series', 7, 4.6, 5, 963.47, 'HPLC system for a variety of liquid chromatography applications.', 'Flexible Configurations', 'HPLC', 'Liquid Chromatography', NULL, 0, product1.png),
('14','BioSpec 70/30 MRI', 8, 4.9, 2, 1269.56, 'MRI system for high-resolution imaging of biological samples.', 'High Resolution', 'MRI', 'Biological Imaging', NULL, 0, product1.png),
('15','BioSpec 94/20 MRI', 8, 4.7, 3, 1964.89, 'MRI system for advanced imaging research.', 'Advanced Features', 'MRI', 'Imaging Research', NULL, 0, product1.png),
('16','S8 TIGER X-Ray Fluorescence', 9, 4.8, 4, 986.78, 'X-Ray fluorescence system for elemental analysis of materials.', 'High Sensitivity', 'X-Ray Fluorescence', 'Elemental Analysis', NULL, 0, product1.png),
('17','BioTools Proteomics Suite', 10, 4.6, 4, 892.45, 'Bioinformatics suite for proteomics and genomics analysis.', 'Comprehensive Analysis', 'Bioinformatics', 'Proteomics Suite', NULL, 0, product1.png),
('18','BioTools Genomics Suite', 10, 4.7, 3, 765.85, 'Suite for genomics research and data analysis.', 'Advanced Research', 'Bioinformatics', 'Genomics Suite', NULL, 0, product1.png),
('19','AFM Dimension Edge', 4, 4.5, 0, 659.86, 'AFM for advanced surface characterization.', 'High Performance', 'AFM', 'Surface Analysis', NULL, 0, product1.png),
('20','LMD6 Laser Microdissection', 5, 4.6, 3, 98.53, 'Laser microdissection for biological samples.', 'Precise Dissection', 'Microdissection', 'Biological Research', NULL, 0, product1.png),
('21','a', 1, 0, 5, 89.56, 'a', 'a', 'a', 'a', NULL, 0, product1.png),
('22','a', 5, 0, 10, 89.56, 'a', 'a', 'a', 'a', NULL, 0, product1.png);


create table CHITIETPHIEUNHAP(
	idCTPN int primary key auto_increment,
    idPN int, 
    idProduct VARCHAR(255),
    DonGiaNhap double, 
    SoLuongNhap int,
    
    foreign key (idPN) references phieunhap(idPN),
    foreign key (idProduct) references Product(ID)
);
INSERT INTO CHITIETPHIEUNHAP (idPN, idProduct, DonGiaNhap, SoLuongNhap) VALUES
(1, 1, 180.00, 10), 
(1, 2, 70.00, 5),    
(2, 3, 90.00, 8),    
(2, 4, 250.00, 12),  
(3, 5, 300.00, 6),   
(3, 6, 900.00, 2),   
(4, 7, 1500.00, 1),  
(4, 8, 1000.00, 3);  

create table OrderDetail(
	ID int primary key auto_increment,
    IdOrder int, 
    IdActiveCode int, 
    IdProduct VARCHAR(255),
    Amount int, 
    priceCode double,
    priceProduct double,
    IdProductVersion int,
    foreign key (IdOrder) references Orders(ID),
    foreign key (IdActiveCode) references ActivationCode(ID),
    foreign key (IdProduct) references Product(ID),
    foreign key (IdProductVersion) references ProductVersion(ID)
);

ALTER TABLE OrderDetail DROP FOREIGN KEY orderdetail_ibfk_4;
ALTER TABLE OrderDetail DROP COLUMN IdProductVersion;

INSERT INTO OrderDetail (IdOrder, IdActiveCode, IdProduct, Amount, priceCode, priceProduct, IdProductVersion) VALUES
(41, 19, 1, 2, 10.00, 190.50, 1), 
(41, 41, 2, 1, 5.00, 75.66, 1),   
(42, 42, 3, 3, 7.00, 96.87, 1),   
(52, 42, 4, 2, 12.00, 269.56, 1), 
(43, 32, 5, 5, 15.00, 306.56, 1), 
(43, 36, 6, 1, 8.00, 910.65, 1),  
(44, NULL, 7, 4, 9.00, 1569.96, 1), 
(44, NULL, 8, 3, 6.00, 963.54, 1); 

DELETE c1
FROM Category c1
INNER JOIN Category c2
WHERE c1.ID > c2.ID
AND c1.CategoryName = c2.CategoryName;

ALTER TABLE Category
ADD CONSTRAINT UC_1 UNIQUE (CategoryName);

DELIMITER $$

CREATE PROCEDURE `GetProcessingOrders` ()
BEGIN
    SELECT orders.ID, orders.IdUser, orders.DCGH, orders.NgayDat, orders.DonHangStatus, orders.SLTong, orders.TongTien
    FROM `orders`
    WHERE orders.DonHangStatus = 'Processing';
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE `GetOrderDetailsById` (IN in_order_id INT)
BEGIN
    SELECT 
    o.ID AS OrderID,
    u.Account AS AccCus,
    c.Name,
    u.Email,
    u.Phone,
    u.Address,
    GROUP_CONCAT(DISTINCT p.Name) AS Products,
    GROUP_CONCAT(DISTINCT od.Amount) AS SLSP,
    GROUP_CONCAT(DISTINCT p.Price) AS GSP,
    GROUP_CONCAT(DISTINCT v.Version) AS Versions,
    SUM(v.Price) AS TotalVersionPrice, -- Tổng giá phiên bản
    o.TongTien AS TongTien,
    o.DonHangStatus
FROM `orders` o
LEFT JOIN `orderdetail` od ON o.ID = od.IdOrder
LEFT JOIN `user` u ON o.IdUser = u.idUser
LEFT JOIN `product` p ON p.ID = od.IdProduct
LEFT JOIN `customer` c ON u.idUser = c.IdUser
LEFT JOIN `productversion` v ON p.IdVersion = v.ID
LEFT JOIN `activationcode` a ON od.IdActiveCode = a.ID
WHERE o.ID = in_order_id
GROUP BY 
    o.ID, 
    u.Account, 
    c.Name, 
    u.Email, 
    u.Phone, 
    u.Address, 
    o.DonHangStatus;
END$$

DELIMITER ;

DELIMITER $$

CREATE TRIGGER UpdateOrderTotalAfterInsert
AFTER INSERT ON OrderDetail
FOR EACH ROW
BEGIN
    DECLARE totalPrice DOUBLE;

    -- Tính tổng tiền cho đơn hàng tương ứng
    SELECT 
        SUM(od.priceProduct * od.Amount) + 
        IFNULL(SUM(v.Price), 0) + 
        IFNULL(SUM(a.Price), 0) INTO totalPrice
    FROM 
        OrderDetail od
    LEFT JOIN 
        product p ON p.ID = od.idProduct   
    LEFT JOIN 
        productversion v ON v.ID = p.IdVersion
    LEFT JOIN 
        ActivationCode a ON od.IdActiveCode = a.ID
    WHERE 
        od.IdOrder = NEW.IdOrder;

    -- Cập nhật giá trị TongTien trong bảng Orders
    UPDATE Orders
    SET TongTien = totalPrice
    WHERE ID = NEW.IdOrder;

    -- Cập nhật SLTong
    UPDATE Orders
    SET SLTong = (SELECT SUM(Amount) FROM OrderDetail WHERE IdOrder = NEW.IdOrder)
    WHERE ID = NEW.IdOrder;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE `CreateOrder` (
    IN `p_IdUser` INT, 
    IN `p_DCGH` VARCHAR(255), 
    IN `p_ThanhToanStatus` VARCHAR(10), 
    IN `p_TotalQuantity` INT, 
    IN `p_TotalPrice` DECIMAL(10,2), 
    OUT `p_OrderId` INT
) 
BEGIN
    DECLARE v_DonHangStatus VARCHAR(10);

    -- Set the order status based on the payment status
    IF p_ThanhToanStatus = 'Napas' THEN
        SET v_DonHangStatus = 'Confirmed';
    ELSEIF p_ThanhToanStatus = 'Credit' THEN
        SET v_DonHangStatus = 'Processing';
    END IF;

    INSERT INTO Orders (IdUser, DCGH, NgayDat, NgayGiao, ThanhToanStatus, DonHangStatus, SLTong, TongTien)
    VALUES (p_IdUser, p_DCGH, CURDATE(), DATE_ADD(CURDATE(), INTERVAL 7 DAY), p_ThanhToanStatus, v_DonHangStatus, p_TotalQuantity, p_TotalPrice);

    SET p_OrderId = LAST_INSERT_ID();
END$$

DELIMITER ;

DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `CreateActiveCode` (
    IN `p_DinhKy` INT, 
    OUT `p_ActiCodeID` INT, 
    OUT `p_ActiCode` VARCHAR(12), 
    OUT `p_Status` VARCHAR(10), 
    OUT `p_NgayKhoiTao` DATE, 
    OUT `p_NgayHetHan` DATE,  
    OUT `p_Price` DECIMAL(10,2)
) 
BEGIN
    DECLARE v_ActiCode VARCHAR(12);
    DECLARE v_Status VARCHAR(10) DEFAULT 'Active';
    DECLARE v_NgayKhoiTao DATE;
    DECLARE v_NgayHetHan DATE;
    DECLARE v_Price DECIMAL(10, 2);
    DECLARE v_ActiCodeID INT;

    -- Tạo mã kích hoạt ngẫu nhiên 12 ký tự
    SET v_ActiCode = SUBSTRING(MD5(RAND()), 1, 12);
    
    -- Ngày khởi tạo là ngày hiện tại
    SET v_NgayKhoiTao = CURDATE();
    
    -- Tính toán Ngày hết hạn và giá dựa trên Định kỳ
    IF p_DinhKy = 6 THEN
        SET v_NgayHetHan = DATE_ADD(v_NgayKhoiTao, INTERVAL 6 MONTH);
        SET v_Price = 89.99;
    ELSEIF p_DinhKy = 12 THEN
        SET v_NgayHetHan = DATE_ADD(v_NgayKhoiTao, INTERVAL 1 YEAR);
        SET v_Price = 267.90;
    ELSEIF p_DinhKy = 24 THEN
        SET v_NgayHetHan = DATE_ADD(v_NgayKhoiTao, INTERVAL 2 YEAR);
        SET v_Price = 719.00;
    ELSE
        -- Nếu Định kỳ không hợp lệ, gán giá trị mặc định
        SET v_NgayHetHan = NULL;
        SET v_Price = 0.00;
    END IF;

    -- Chèn bản ghi vào bảng activationcode
    INSERT INTO activationcode (ActiCode, Status, NgayKhoiTao, NgayHetHan, DinhKy, Price)
    VALUES (v_ActiCode, v_Status, v_NgayKhoiTao, v_NgayHetHan, CAST(p_DinhKy AS CHAR), v_Price);

    -- Lấy ID của bản ghi vừa chèn
    SET v_ActiCodeID = LAST_INSERT_ID();

    -- Cập nhật các tham số đầu ra
    SET p_ActiCodeID = v_ActiCodeID;
    SET p_ActiCode = v_ActiCode;
    SET p_Status = v_Status;
    SET p_NgayKhoiTao = v_NgayKhoiTao;
    SET p_NgayHetHan = v_NgayHetHan;
    SET p_Price = v_Price;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE `CreateOrderDetail` (IN `p_OrderId` INT, IN `v_ActiCode` INT, IN `p_ProductData` JSON) 
BEGIN
    DECLARE v_IdProduct VARCHAR(255);
    DECLARE v_Quantity INT;
    DECLARE v_priceCode DOUBLE;
    DECLARE v_priceProduct DOUBLE;
    DECLARE v_ProductSL INT;

    DECLARE idx INT DEFAULT 0;
    DECLARE arrLength INT;

    -- Lấy độ dài của mảng JSON
    SET arrLength = JSON_LENGTH(p_ProductData);
    
    read_loop: LOOP
        -- Thoát khỏi vòng lặp nếu đã đọc đủ các sản phẩm
        IF idx >= arrLength THEN
            LEAVE read_loop;
        END IF;

        -- Trích xuất ID sản phẩm và số lượng từ mảng JSON
        SET v_IdProduct = JSON_UNQUOTE(JSON_EXTRACT(p_ProductData, CONCAT('$[', idx, '].IdProduct')));
        SET v_Quantity = JSON_EXTRACT(p_ProductData, CONCAT('$[', idx, '].Amount'));
        SET v_priceCode = JSON_UNQUOTE(JSON_EXTRACT(p_ProductData, CONCAT('$[', idx, '].priceCode')));
        SET v_priceProduct = JSON_UNQUOTE(JSON_EXTRACT(p_ProductData, CONCAT('$[', idx, '].priceProduct'))); 

        -- Chèn chi tiết đơn hàng
        INSERT INTO OrderDetail (IdOrder, IdActiveCode, idProduct, Amount, priceCode, priceProduct)
        VALUES (p_OrderId, v_ActiCode, v_IdProduct, v_Quantity, v_priceCode, v_priceProduct);

        -- Cập nhật số lượng tồn kho (SL) trong bảng Product
        -- Lấy số lượng tồn kho hiện tại
        SELECT SL INTO v_ProductSL FROM Product WHERE ID = v_IdProduct;
		
        -- Cập nhật số lượng tồn kho
        UPDATE Product
        SET SL = SL - v_Quantity
        WHERE ID = v_IdProduct;

        SET idx = idx + 1;
    END LOOP;

END$$

DELIMITER ;

DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetOrderDetailsByUserId` (IN `userId` INT)
BEGIN
    SELECT
        o.ID AS OrderID,
        p.image AS FirstImage,
        p.Name AS FirstProductName,
        o.NgayDat AS OrderDate,
        o.NgayGiao AS DeliveryDate,
        o.DonHangStatus AS OrderStatus
    FROM
        orders o
        JOIN orderdetail od ON o.ID = od.IdOrder
        JOIN product p ON od.idProduct = p.ID
    WHERE
        o.IdUser = userId
        AND od.ID = (
            SELECT MIN(od2.ID)
            FROM orderdetail od2
            WHERE od2.IdOrder = o.ID
        )
    ORDER BY
        o.ID DESC;
END$$

DELIMITER ;


INSERT INTO OrderDetail (IdOrder, IdActiveCode, IdProduct, Amount, priceCode, priceProduct)
VALUES (55, 19, '1', 3, 89.99, 190.5);

INSERT INTO OrderDetail (IdOrder, IdActiveCode, IdProduct, Amount, priceCode, priceProduct)
VALUES (55, 20, '4', 2, 89.99, 269.56);
