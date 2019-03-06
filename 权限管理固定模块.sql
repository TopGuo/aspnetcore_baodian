/*
MySQL Backup
Source Server Version: 5.7.24
Source Database: RestApi
Date: 2018/12/27 14:59:42
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
--  Table structure for `backstage_user`
-- ----------------------------
DROP TABLE IF EXISTS `backstage_user`;
CREATE TABLE `backstage_user` (
  `Id` varchar(36) NOT NULL DEFAULT '',
  `LoginName` varchar(50) NOT NULL DEFAULT '' COMMENT '登录用户',
  `Password` varchar(50) NOT NULL DEFAULT '123456' COMMENT '密码',
  `LastLoginIp` varchar(50) NOT NULL DEFAULT '' COMMENT '上次登录Ip',
  `Email` varchar(50) NOT NULL DEFAULT '' COMMENT '邮箱',
  `UpdateTime` datetime DEFAULT NULL COMMENT '修改时间',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `LastLoginTime` datetime DEFAULT NULL COMMENT '上次登录时间',
  `RoleId` int(11) NOT NULL DEFAULT '0' COMMENT '角色Id',
  `FullName` varchar(50) DEFAULT '' COMMENT '全名',
  `Mobile` varchar(20) NOT NULL DEFAULT '' COMMENT '手机号',
  `Gender` int(1) DEFAULT '0' COMMENT '性别',
  `IdCard` varchar(50) DEFAULT '' COMMENT '身份证',
  `AccountType` int(11) NOT NULL DEFAULT '0' COMMENT '用户类别',
  `AccountStatus` int(11) NOT NULL DEFAULT '0' COMMENT '用户状态',
  `SourceType` int(11) NOT NULL DEFAULT '0' COMMENT '来源渠道',
  PRIMARY KEY (`Id`),
  KEY `rId` (`RoleId`),
  CONSTRAINT `rId` FOREIGN KEY (`RoleId`) REFERENCES `system_roles` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
--  Table structure for `system_actions`
-- ----------------------------
DROP TABLE IF EXISTS `system_actions`;
CREATE TABLE `system_actions` (
  `ActionId` varchar(36) NOT NULL DEFAULT '' COMMENT '权限Id',
  `ActionDescription` varchar(100) NOT NULL DEFAULT '' COMMENT '权限描述',
  `ActionName` varchar(255) NOT NULL DEFAULT '' COMMENT '权限名称',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `Url` varchar(255) NOT NULL DEFAULT '' COMMENT 'URL',
  `Orders` int(11) DEFAULT NULL COMMENT '排序',
  `ParentAction` varchar(255) DEFAULT NULL COMMENT '父级权限',
  PRIMARY KEY (`ActionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
--  Table structure for `system_role_permission`
-- ----------------------------
DROP TABLE IF EXISTS `system_role_permission`;
CREATE TABLE `system_role_permission` (
  `RoleId` int(11) NOT NULL DEFAULT '0' COMMENT '角色Id',
  `ActionId` varchar(36) NOT NULL COMMENT '权限Id',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`RoleId`,`ActionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
--  Table structure for `system_roles`
-- ----------------------------
DROP TABLE IF EXISTS `system_roles`;
CREATE TABLE `system_roles` (
  `Id` int(11) NOT NULL DEFAULT '0' COMMENT '角色Id',
  `Name` varchar(50) NOT NULL DEFAULT '' COMMENT '角色名称',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
--  Table structure for `task_schedule`
-- ----------------------------
DROP TABLE IF EXISTS `task_schedule`;
CREATE TABLE `task_schedule` (
  `Id` varchar(36) NOT NULL DEFAULT '' COMMENT 'id',
  `JobGroup` varchar(50) NOT NULL DEFAULT '' COMMENT '任务组',
  `JobName` varchar(100) NOT NULL DEFAULT '' COMMENT '任务名',
  `CronExpress` varchar(50) DEFAULT NULL COMMENT 'CronExpress表达式',
  `StarRunTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '开始允许时间',
  `EndRunTime` datetime DEFAULT NULL COMMENT '结束允许时间',
  `NextRunTime` datetime DEFAULT NULL COMMENT '下次运行时间',
  `RunStatus` int(11) NOT NULL DEFAULT '0' COMMENT '运行状态',
  `Remark` text COMMENT '任务描述备注',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateTime` datetime DEFAULT NULL COMMENT '修改时间',
  `CreateAuthr` varchar(50) NOT NULL DEFAULT '' COMMENT '操作员',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
--  Records 
-- ----------------------------
INSERT INTO `backstage_user` VALUES ('050899e3f16445ecb05f07b0bb71d90e','guo','E10ADC3949BA59ABBE56E057F20F883E','','','2018-12-15 06:21:36','2018-11-21 16:58:55',NULL,'2','鸟窝','18333103619','0','130726199512263114','2','1','1'), ('3c317eec668a491c8d473c9a2404c4fc','阿里巴巴','E10ADC3949BA59ABBE56E057F20F883E','','',NULL,'2018-12-08 02:51:40',NULL,'64','阿里妈妈','12345678900','0','130726199512263333','64','1','1'), ('7d127bbfae1344a1ba739b3ce750758f','guocaiwu','E10ADC3949BA59ABBE56E057F20F883E','','','2018-11-22 11:07:42','2018-11-21 19:05:16','2018-11-21 19:47:52','4','财务','18333103619','0','130726199512263114','0','0','1'), ('84948cdf710c4b6491f9f508f5010d9b','XLXLXL','E10ADC3949BA59ABBE56E057F20F883E','','','2018-11-24 21:06:30','2018-11-24 21:04:44','2018-11-25 20:18:17','32','小李','18333103619','0','130726199512263333','32','1','1'), ('9e6c0fd0476c4d66a5941427b9c4d435','kfkfkf','E10ADC3949BA59ABBE56E057F20F883E','','',NULL,'2018-11-25 12:03:42','2018-11-25 12:10:19','8','客服','18333103619','0','130726199512263333','8','1','1'), ('a61691daa89b44a4994c5a18a409e214','admin','E10ADC3949BA59ABBE56E057F20F883E','','','2018-11-24 01:50:01','2018-11-21 17:27:18','2018-12-27 04:22:47','1','鸟窝','18333103619','0','130726199512263114','1','1','1'), ('eae2522c080e411a8d5fd5e446addcb9','xiaoshou','E10ADC3949BA59ABBE56E057F20F883E','','','2018-11-28 01:20:52','2018-11-24 12:53:20','2018-11-24 14:01:38','16','销售','18333103619','0','130726199512263333','16','0','1'), ('ef77a8780fae47989ac98717a0c8be17','ccputong','96E79218965EB72C92A549DD5A330112','','','2018-11-25 11:27:41','2018-11-24 02:21:07','2018-11-24 02:51:20','2','菜鸟','18333103619','0','130726199512263333','2','1','1');
INSERT INTO `system_actions` VALUES ('00000000-0000-0000-0000-000000000001','系统管理','系统管理','2018-12-25 11:52:37','','1',NULL), ('00000000-0000-0000-0000-000000000002','用户管理','用户管理','2018-12-25 11:52:37','','2',NULL), ('00000000-0000-0000-0000-000000000003','财务管理','财务管理','2018-12-25 11:52:37','','3',NULL), ('00000000-0000-0000-0000-000000000004','测试杂谈','测试杂谈','2018-12-25 11:52:37','','4',NULL), ('1e3fe206feac443ba8fda56839dadabe','任务管理','TB.AspNetCore.WebSite.Controllers.TaskInfoController.Index (TB.AspNetCore.WebSite)','2018-11-23 16:40:10','/TaskInfo/Index','3','00000000-0000-0000-0000-000000000001'), ('99f8cfb043d94df8b2044ae8f992ff23','角色管理','TB.AspNetCore.WebSite.Controllers.HomeController.Roles (TB.AspNetCore.WebSite)','2018-11-21 10:09:50','/Home/Roles','1','00000000-0000-0000-0000-000000000001'), ('a4e82b6e783146a699a0ed4a8cbb06d6','操作员管理','TB.AspNetCore.WebSite.Controllers.MemberController.BackstageUser (TB.AspNetCore.WebSite)','2018-11-21 11:53:25','/Member/BackstageUser','2','00000000-0000-0000-0000-000000000001');
INSERT INTO `system_role_permission` VALUES ('1','00000000-0000-0000-0000-000000000001','2018-11-21 10:12:00'), ('1','00000000-0000-0000-0000-000000000002','2018-11-21 10:12:00'), ('1','00000000-0000-0000-0000-000000000003','2018-11-23 20:31:09'), ('1','00000000-0000-0000-0000-000000000004','2018-11-25 18:57:45'), ('1','1e3fe206feac443ba8fda56839dadabe','2018-11-23 16:40:13'), ('1','99f8cfb043d94df8b2044ae8f992ff23','2018-11-21 10:12:00'), ('1','a4e82b6e783146a699a0ed4a8cbb06d6','2018-11-21 11:53:28'), ('2','1e3fe206feac443ba8fda56839dadabe','2018-11-24 02:22:17'), ('2','a4e82b6e783146a699a0ed4a8cbb06d6','2018-11-26 07:17:52'), ('4','99f8cfb043d94df8b2044ae8f992ff23','2018-11-21 16:04:43'), ('16','a4e82b6e783146a699a0ed4a8cbb06d6','2018-11-24 12:54:56'), ('256','1e3fe206feac443ba8fda56839dadabe','2018-12-19 12:33:14');
INSERT INTO `system_roles` VALUES ('1','超级管理员'), ('2','普通用户'), ('4','财务总监'), ('8','客服'), ('16','销售'), ('32','财务'), ('64','技术员'), ('128','test'), ('256','test2');
INSERT INTO `task_schedule` VALUES ('1','12','12','0/30 * * * * ? ','2018-11-23 05:32:00','9999-12-31 00:00:00',NULL,'2',NULL,'2018-11-23 05:32:29','2018-12-15 06:21:51','admin'), ('5328599c0b6747ea99674fa6b4a242ea','132','78','0/30 * * * * ?','2018-10-02 00:00:00','2018-11-29 00:00:00',NULL,'0',NULL,'2018-11-29 13:27:49','2018-12-08 05:46:34','admin'), ('55f1617046874856a5fd0122f4e229d8','group','1','0/10 * * * * ?','2018-12-11 09:20:00','9999-12-31 00:00:00',NULL,'4',NULL,'2018-12-11 09:20:59','2018-12-11 16:07:38','admin');
