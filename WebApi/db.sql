/*
SQLyog Ultimate v13.1.1 (64 bit)
MySQL - 5.7.20-log : Database - alischoolcard
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`alischoolcard` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `alischoolcard`;

/*Table structure for table `_tb_payment_alipay_record_temp` */

DROP TABLE IF EXISTS `_tb_payment_alipay_record_temp`;

CREATE TABLE `_tb_payment_alipay_record_temp` (
  `alipay_order` varchar(50) NOT NULL DEFAULT '',
  `order` varchar(50) NOT NULL DEFAULT '',
  `type` int(4) NOT NULL DEFAULT '0',
  `schoolcode` varchar(50) DEFAULT '',
  `create_time` datetime NOT NULL,
  PRIMARY KEY (`alipay_order`,`order`,`type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `_tb_payment_item_temp` */

DROP TABLE IF EXISTS `_tb_payment_item_temp`;

CREATE TABLE `_tb_payment_item_temp` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `schoolcode` varchar(50) DEFAULT '',
  `name` varchar(50) NOT NULL DEFAULT '',
  `is_external` int(11) NOT NULL DEFAULT '0',
  `fixed` int(11) DEFAULT '0',
  `account` varchar(100) NOT NULL DEFAULT '',
  `target` int(11) NOT NULL DEFAULT '0',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00',
  `type` int(11) NOT NULL DEFAULT '0',
  `introduction` varchar(1000) NOT NULL DEFAULT '',
  `icon` int(11) NOT NULL DEFAULT '0',
  `group` int(11) NOT NULL DEFAULT '0',
  `method` int(11) NOT NULL DEFAULT '0',
  `can_set_count` int(4) NOT NULL DEFAULT '0',
  `nessary_info` varchar(100) NOT NULL DEFAULT '',
  `date_from` datetime NOT NULL,
  `date_to` datetime NOT NULL,
  `count` int(11) NOT NULL DEFAULT '0',
  `notify_link` varchar(200) DEFAULT '',
  `notify_key` varchar(50) DEFAULT '',
  `notify_msg` varchar(200) DEFAULT '',
  `remark` varchar(200) DEFAULT '',
  `status` int(4) NOT NULL DEFAULT '0',
  `limit` int(11) DEFAULT '0',
  `class_id` int(11) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=116 DEFAULT CHARSET=utf8;

/*Table structure for table `dts_log_heart_beat` */

DROP TABLE IF EXISTS `dts_log_heart_beat`;

CREATE TABLE `dts_log_heart_beat` (
  `thread_id` int(11) NOT NULL DEFAULT '0',
  `heartbeat` varchar(64) DEFAULT '',
  PRIMARY KEY (`thread_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `schoolmid` */

DROP TABLE IF EXISTS `schoolmid`;

CREATE TABLE `schoolmid` (
  `所属院校` varchar(255) DEFAULT '',
  `对应商家中心账号` varchar(255) DEFAULT '',
  `mid` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`mid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `sh_area` */

DROP TABLE IF EXISTS `sh_area`;

CREATE TABLE `sh_area` (
  `id` bigint(20) NOT NULL DEFAULT '0',
  `pid` int(11) DEFAULT NULL,
  `shortname` varchar(100) DEFAULT 'NULL',
  `name` varchar(100) DEFAULT 'NULL',
  `merger_name` varchar(255) DEFAULT 'NULL',
  `level` int(11) DEFAULT NULL,
  `pinyin` varchar(100) DEFAULT 'NULL',
  `code` varchar(100) DEFAULT 'NULL',
  `zip_code` varchar(100) DEFAULT 'NULL',
  `first` varchar(50) DEFAULT 'NULL',
  `lng` varchar(100) DEFAULT 'NULL',
  `lat` varchar(100) DEFAULT 'NULL',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_ali_default_sign` */

DROP TABLE IF EXISTS `tb_ali_default_sign`;

CREATE TABLE `tb_ali_default_sign` (
  `ali_sign_id` int(11) NOT NULL AUTO_INCREMENT,
  `merchant_id` int(11) DEFAULT '0',
  `payment_item_id` int(11) DEFAULT '0',
  `app_auth_token` varchar(50) DEFAULT '',
  `auth_app_id` varchar(50) DEFAULT '',
  `user_id` varchar(50) DEFAULT '',
  `expires_in` varchar(50) DEFAULT '',
  `re_expires_in` varchar(50) DEFAULT '',
  `state` int(11) DEFAULT '0',
  `update_date` datetime DEFAULT NULL,
  `create_date` datetime DEFAULT NULL,
  PRIMARY KEY (`ali_sign_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_alipay_image` */

DROP TABLE IF EXISTS `tb_alipay_image`;

CREATE TABLE `tb_alipay_image` (
  `image_id` int(11) NOT NULL AUTO_INCREMENT,
  `alipay_id` varchar(64) DEFAULT '',
  `alipay_url` varchar(128) DEFAULT '',
  `url` varchar(128) DEFAULT '',
  `type` int(11) DEFAULT '0',
  PRIMARY KEY (`image_id`)
) ENGINE=InnoDB AUTO_INCREMENT=250 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_alipay_school` */

DROP TABLE IF EXISTS `tb_alipay_school`;

CREATE TABLE `tb_alipay_school` (
  `School_ID` int(11) NOT NULL DEFAULT '0',
  `School_name` varchar(60) DEFAULT '',
  `School_Main_Admin_name` varchar(20) DEFAULT '',
  `School_Main_Admin_password` varchar(64) DEFAULT '',
  `School_Bool_Main_Admin` int(11) DEFAULT '0',
  `School_Address` varchar(100) DEFAULT '',
  `School_Collar_card_link` varchar(100) DEFAULT '',
  `user_id` varchar(20) DEFAULT '',
  `auth_app_id` varchar(20) DEFAULT '',
  `app_auth_token` varchar(64) DEFAULT '',
  PRIMARY KEY (`School_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_alipaymarketingcampaigncash` */

DROP TABLE IF EXISTS `tb_alipaymarketingcampaigncash`;

CREATE TABLE `tb_alipaymarketingcampaigncash` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `coupon_name` varchar(32) DEFAULT '',
  `prize_type` varchar(6) DEFAULT '',
  `total_money` double DEFAULT '0',
  `total_num` int(11) DEFAULT '0',
  `prize_msg` varchar(64) DEFAULT '',
  `start_time` datetime DEFAULT NULL,
  `end_time` datetime DEFAULT NULL,
  `merchant_link` varchar(64) DEFAULT '',
  `send_freqency` varchar(20) DEFAULT '',
  `crowd_no` varchar(128) DEFAULT '',
  `pay_url` varchar(256) DEFAULT '',
  `origin_crowd_no` varchar(64) DEFAULT '',
  `count` int(11) DEFAULT '0',
  `camp_status` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_alipaymarketingcampaigncashtrigger` */

DROP TABLE IF EXISTS `tb_alipaymarketingcampaigncashtrigger`;

CREATE TABLE `tb_alipaymarketingcampaigncashtrigger` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` varchar(40) DEFAULT '',
  `crowd_no` varchar(128) DEFAULT '',
  `trigger_result` varchar(8) DEFAULT '',
  `prize_amount` double DEFAULT '0',
  `repeat_trigger_flag` varchar(8) DEFAULT '',
  `partner_id` varchar(40) DEFAULT '',
  `error_msg` varchar(200) DEFAULT '',
  `coupon_name` varchar(32) DEFAULT '',
  `prize_msg` varchar(64) DEFAULT '',
  `merchant_logo` varchar(200) DEFAULT '',
  `biz_no` varchar(96) DEFAULT '',
  `out_biz_no` varchar(96) DEFAULT '',
  `pay_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4872 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_ammeter` */

DROP TABLE IF EXISTS `tb_ammeter`;

CREATE TABLE `tb_ammeter` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `MeterAddr` varchar(50) DEFAULT '',
  `room_id` int(11) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `unique_tb_ammeter` (`room_id`,`MeterAddr`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2832 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_app_maintain` */

DROP TABLE IF EXISTS `tb_app_maintain`;

CREATE TABLE `tb_app_maintain` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `appid` varchar(50) DEFAULT NULL,
  `endTime` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_appaccounts_item` */

DROP TABLE IF EXISTS `tb_appaccounts_item`;

CREATE TABLE `tb_appaccounts_item` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `appId` varchar(50) DEFAULT '',
  `typename` varchar(50) DEFAULT '',
  `schoolcode` varchar(50) DEFAULT '',
  `accounts_id` int(11) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_attendance` */

DROP TABLE IF EXISTS `tb_attendance`;

CREATE TABLE `tb_attendance` (
  `id` bigint(20) NOT NULL DEFAULT '0',
  `teamID` bigint(20) DEFAULT '0',
  `joinUserid` varchar(20) DEFAULT '',
  `attendanceTime` datetime DEFAULT NULL,
  `attendanceType` int(11) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_building_room_config` */

DROP TABLE IF EXISTS `tb_building_room_config`;

CREATE TABLE `tb_building_room_config` (
  `id` bigint(20) NOT NULL DEFAULT '0',
  `school_id` int(11) DEFAULT '0',
  `building_room_no` varchar(50) DEFAULT '',
  `parent_id` bigint(20) DEFAULT '0',
  `ispublic` int(11) DEFAULT '0',
  `building_room_num` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`),
  KEY `unique_tb_Building_Room_Config` (`building_room_no`,`school_id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_cashier_device` */

DROP TABLE IF EXISTS `tb_cashier_device`;

CREATE TABLE `tb_cashier_device` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sn` varchar(100) NOT NULL DEFAULT '',
  `brand` int(11) NOT NULL DEFAULT '0',
  `stall` int(11) NOT NULL DEFAULT '0',
  `schoolcode` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`),
  KEY `sn` (`sn`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=1791 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_cashier_dining_hall` */

DROP TABLE IF EXISTS `tb_cashier_dining_hall`;

CREATE TABLE `tb_cashier_dining_hall` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL DEFAULT '',
  `introduction` varchar(300) NOT NULL DEFAULT '',
  `schoolcode` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=58 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_cashier_merchant` */

DROP TABLE IF EXISTS `tb_cashier_merchant`;

CREATE TABLE `tb_cashier_merchant` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL DEFAULT '',
  `department` varchar(50) NOT NULL DEFAULT '',
  `alipay_account` varchar(200) NOT NULL DEFAULT '',
  `status` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_cashier_merchant_stall` */

DROP TABLE IF EXISTS `tb_cashier_merchant_stall`;

CREATE TABLE `tb_cashier_merchant_stall` (
  `merchant_id` int(11) NOT NULL DEFAULT '0',
  `stall_id` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`merchant_id`,`stall_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_cashier_stall` */

DROP TABLE IF EXISTS `tb_cashier_stall`;

CREATE TABLE `tb_cashier_stall` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL DEFAULT '',
  `type` tinyint(4) NOT NULL DEFAULT '0',
  `dining_tall` int(11) NOT NULL DEFAULT '0',
  `school_card_rate` int(11) NOT NULL DEFAULT '0',
  `student_card_rate` int(11) NOT NULL DEFAULT '0',
  `other_rate` int(11) NOT NULL DEFAULT '0',
  `refund` tinyint(4) NOT NULL DEFAULT '0',
  `payee_account` int(11) NOT NULL DEFAULT '0',
  `subsidy` int(11) NOT NULL DEFAULT '0',
  `refund_password` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1640 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_cashier_sumcount` */

DROP TABLE IF EXISTS `tb_cashier_sumcount`;

CREATE TABLE `tb_cashier_sumcount` (
  `days` varchar(200) NOT NULL,
  `paysum` decimal(18,2) DEFAULT NULL,
  `ordercount` int(4) DEFAULT NULL,
  `school_id` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_cashier_trade_order` */

DROP TABLE IF EXISTS `tb_cashier_trade_order`;

CREATE TABLE `tb_cashier_trade_order` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `order` varchar(128) NOT NULL DEFAULT '',
  `user_code` varchar(50) NOT NULL DEFAULT '',
  `name` varchar(50) NOT NULL DEFAULT '',
  `shop` int(11) NOT NULL DEFAULT '0',
  `stall` int(11) NOT NULL DEFAULT '0',
  `machine` int(11) NOT NULL DEFAULT '0',
  `paid` decimal(18,2) NOT NULL DEFAULT '0.00',
  `refund` decimal(18,2) NOT NULL DEFAULT '0.00',
  `status` tinyint(4) NOT NULL DEFAULT '0',
  `payer_account` varchar(50) DEFAULT '',
  `pay_amount` decimal(10,0) DEFAULT '0',
  `alipay_order` varchar(128) NOT NULL DEFAULT '',
  `type` int(11) DEFAULT '0',
  `trade_name` varchar(50) DEFAULT '',
  `create_time` datetime NOT NULL,
  `finish_time` datetime NOT NULL,
  `operator` varchar(160) DEFAULT '',
  `terminal_number` varchar(128) DEFAULT '',
  `alipay_red` decimal(10,0) DEFAULT '0',
  `collection_treasure` decimal(10,0) DEFAULT '0',
  `alipay_discount` decimal(10,0) DEFAULT '0',
  `merchant_discount` decimal(10,0) DEFAULT '0',
  `ticket_money` decimal(10,0) DEFAULT '0',
  `ticket_name` varchar(50) DEFAULT '',
  `merchant_red_consumption` decimal(10,0) DEFAULT '0',
  `card_consumption` decimal(10,0) DEFAULT '0',
  `refund_batch_number` varchar(50) DEFAULT '',
  `service_charge` decimal(10,0) DEFAULT '0',
  `shares_profit` decimal(10,0) DEFAULT '0',
  `remark` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`,`finish_time`),
  KEY `id` (`id`),
  KEY `UQ__tb_cashier_trade__63A48FA2` (`order`),
  KEY `status` (`status`) USING BTREE,
  KEY `type` (`type`) USING BTREE,
  KEY `create_time` (`create_time`) USING BTREE,
  KEY `finish_time` (`finish_time`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=467004 DEFAULT CHARSET=utf8
/*!50100 PARTITION BY RANGE (TO_DAYS(finish_time))
(PARTITION p20181001 VALUES LESS THAN (737333) ENGINE = InnoDB,
 PARTITION p20181002 VALUES LESS THAN (737334) ENGINE = InnoDB,
 PARTITION p20181003 VALUES LESS THAN (737335) ENGINE = InnoDB,
 PARTITION p20181004 VALUES LESS THAN (737336) ENGINE = InnoDB,
 PARTITION p20181005 VALUES LESS THAN (737337) ENGINE = InnoDB,
 PARTITION p20181006 VALUES LESS THAN (737338) ENGINE = InnoDB,
 PARTITION p20181007 VALUES LESS THAN (737339) ENGINE = InnoDB,
 PARTITION p20181008 VALUES LESS THAN (737340) ENGINE = InnoDB,
 PARTITION p20181009 VALUES LESS THAN (737341) ENGINE = InnoDB,
 PARTITION p20181010 VALUES LESS THAN (737342) ENGINE = InnoDB,
 PARTITION p20181011 VALUES LESS THAN (737343) ENGINE = InnoDB,
 PARTITION p20181012 VALUES LESS THAN (737344) ENGINE = InnoDB,
 PARTITION p20181013 VALUES LESS THAN (737345) ENGINE = InnoDB,
 PARTITION p20181014 VALUES LESS THAN (737346) ENGINE = InnoDB,
 PARTITION p20181015 VALUES LESS THAN (737347) ENGINE = InnoDB,
 PARTITION p20181016 VALUES LESS THAN (737348) ENGINE = InnoDB,
 PARTITION p20181017 VALUES LESS THAN (737349) ENGINE = InnoDB,
 PARTITION p20181018 VALUES LESS THAN (737350) ENGINE = InnoDB,
 PARTITION p20181019 VALUES LESS THAN (737351) ENGINE = InnoDB,
 PARTITION p20181020 VALUES LESS THAN (737352) ENGINE = InnoDB,
 PARTITION p20181021 VALUES LESS THAN (737353) ENGINE = InnoDB,
 PARTITION p20181022 VALUES LESS THAN (737354) ENGINE = InnoDB,
 PARTITION p20181023 VALUES LESS THAN (737355) ENGINE = InnoDB,
 PARTITION p20181024 VALUES LESS THAN (737356) ENGINE = InnoDB,
 PARTITION p20181025 VALUES LESS THAN (737357) ENGINE = InnoDB,
 PARTITION p20181026 VALUES LESS THAN (737358) ENGINE = InnoDB,
 PARTITION p20181027 VALUES LESS THAN (737359) ENGINE = InnoDB,
 PARTITION p20181028 VALUES LESS THAN (737360) ENGINE = InnoDB,
 PARTITION p20181029 VALUES LESS THAN (737361) ENGINE = InnoDB,
 PARTITION p20181030 VALUES LESS THAN (737362) ENGINE = InnoDB,
 PARTITION p20181031 VALUES LESS THAN (737363) ENGINE = InnoDB,
 PARTITION p20181101 VALUES LESS THAN (737364) ENGINE = InnoDB,
 PARTITION p20181102 VALUES LESS THAN (737365) ENGINE = InnoDB,
 PARTITION p20181103 VALUES LESS THAN (737366) ENGINE = InnoDB,
 PARTITION p20181104 VALUES LESS THAN (737367) ENGINE = InnoDB,
 PARTITION p20181105 VALUES LESS THAN (737368) ENGINE = InnoDB,
 PARTITION p20181106 VALUES LESS THAN (737369) ENGINE = InnoDB,
 PARTITION p20181107 VALUES LESS THAN (737370) ENGINE = InnoDB,
 PARTITION p20181108 VALUES LESS THAN (737371) ENGINE = InnoDB,
 PARTITION p20181109 VALUES LESS THAN (737372) ENGINE = InnoDB,
 PARTITION p20181110 VALUES LESS THAN (737373) ENGINE = InnoDB,
 PARTITION p20181111 VALUES LESS THAN (737374) ENGINE = InnoDB,
 PARTITION p20181112 VALUES LESS THAN (737375) ENGINE = InnoDB,
 PARTITION p20181113 VALUES LESS THAN (737376) ENGINE = InnoDB,
 PARTITION p20181114 VALUES LESS THAN (737377) ENGINE = InnoDB,
 PARTITION p20181115 VALUES LESS THAN (737378) ENGINE = InnoDB,
 PARTITION p20181116 VALUES LESS THAN (737379) ENGINE = InnoDB,
 PARTITION p20181117 VALUES LESS THAN (737380) ENGINE = InnoDB,
 PARTITION p20181118 VALUES LESS THAN (737381) ENGINE = InnoDB,
 PARTITION p20181119 VALUES LESS THAN (737382) ENGINE = InnoDB,
 PARTITION p20181120 VALUES LESS THAN (737383) ENGINE = InnoDB,
 PARTITION p20181121 VALUES LESS THAN (737384) ENGINE = InnoDB,
 PARTITION p20181122 VALUES LESS THAN (737385) ENGINE = InnoDB,
 PARTITION p20181123 VALUES LESS THAN (737386) ENGINE = InnoDB,
 PARTITION p20181124 VALUES LESS THAN (737387) ENGINE = InnoDB,
 PARTITION p20181125 VALUES LESS THAN (737388) ENGINE = InnoDB,
 PARTITION p20181126 VALUES LESS THAN (737389) ENGINE = InnoDB,
 PARTITION p20181127 VALUES LESS THAN (737390) ENGINE = InnoDB,
 PARTITION p20181128 VALUES LESS THAN (737391) ENGINE = InnoDB,
 PARTITION p20181129 VALUES LESS THAN (737392) ENGINE = InnoDB,
 PARTITION p20181130 VALUES LESS THAN (737393) ENGINE = InnoDB,
 PARTITION p20181201 VALUES LESS THAN (737394) ENGINE = InnoDB,
 PARTITION p20181202 VALUES LESS THAN (737395) ENGINE = InnoDB,
 PARTITION p20181203 VALUES LESS THAN (737396) ENGINE = InnoDB,
 PARTITION p20181204 VALUES LESS THAN (737397) ENGINE = InnoDB,
 PARTITION p20181205 VALUES LESS THAN (737398) ENGINE = InnoDB,
 PARTITION p20181206 VALUES LESS THAN (737399) ENGINE = InnoDB,
 PARTITION p20181207 VALUES LESS THAN (737400) ENGINE = InnoDB,
 PARTITION p20181208 VALUES LESS THAN (737401) ENGINE = InnoDB,
 PARTITION p20181209 VALUES LESS THAN (737402) ENGINE = InnoDB,
 PARTITION p20181210 VALUES LESS THAN (737403) ENGINE = InnoDB,
 PARTITION p20181211 VALUES LESS THAN (737404) ENGINE = InnoDB,
 PARTITION p20181212 VALUES LESS THAN (737405) ENGINE = InnoDB,
 PARTITION p20181213 VALUES LESS THAN (737406) ENGINE = InnoDB,
 PARTITION p20181214 VALUES LESS THAN (737407) ENGINE = InnoDB,
 PARTITION p20181215 VALUES LESS THAN (737408) ENGINE = InnoDB,
 PARTITION p20181216 VALUES LESS THAN (737409) ENGINE = InnoDB,
 PARTITION p20181217 VALUES LESS THAN (737410) ENGINE = InnoDB,
 PARTITION p20181218 VALUES LESS THAN (737411) ENGINE = InnoDB,
 PARTITION p20181219 VALUES LESS THAN (737412) ENGINE = InnoDB,
 PARTITION p20181220 VALUES LESS THAN (737413) ENGINE = InnoDB,
 PARTITION p20181221 VALUES LESS THAN (737414) ENGINE = InnoDB,
 PARTITION p20181222 VALUES LESS THAN (737415) ENGINE = InnoDB,
 PARTITION p20181223 VALUES LESS THAN (737416) ENGINE = InnoDB,
 PARTITION p20181224 VALUES LESS THAN (737417) ENGINE = InnoDB,
 PARTITION p20181225 VALUES LESS THAN (737418) ENGINE = InnoDB,
 PARTITION p20181226 VALUES LESS THAN (737419) ENGINE = InnoDB,
 PARTITION p20181227 VALUES LESS THAN (737420) ENGINE = InnoDB,
 PARTITION p20181228 VALUES LESS THAN (737421) ENGINE = InnoDB,
 PARTITION p20181229 VALUES LESS THAN (737422) ENGINE = InnoDB,
 PARTITION p20181230 VALUES LESS THAN (737423) ENGINE = InnoDB,
 PARTITION p20181231 VALUES LESS THAN (737424) ENGINE = InnoDB,
 PARTITION p20190101 VALUES LESS THAN (737425) ENGINE = InnoDB,
 PARTITION p20190102 VALUES LESS THAN (737426) ENGINE = InnoDB,
 PARTITION p20190103 VALUES LESS THAN (737427) ENGINE = InnoDB,
 PARTITION p20190104 VALUES LESS THAN (737428) ENGINE = InnoDB,
 PARTITION p20190105 VALUES LESS THAN (737429) ENGINE = InnoDB,
 PARTITION p20190106 VALUES LESS THAN (737430) ENGINE = InnoDB,
 PARTITION p20190107 VALUES LESS THAN (737431) ENGINE = InnoDB,
 PARTITION p20190108 VALUES LESS THAN (737432) ENGINE = InnoDB,
 PARTITION p20190109 VALUES LESS THAN (737433) ENGINE = InnoDB,
 PARTITION p20190110 VALUES LESS THAN (737434) ENGINE = InnoDB,
 PARTITION p20190111 VALUES LESS THAN (737435) ENGINE = InnoDB,
 PARTITION p20190112 VALUES LESS THAN (737436) ENGINE = InnoDB,
 PARTITION p20190113 VALUES LESS THAN (20190114) ENGINE = InnoDB,
 PARTITION p20190114 VALUES LESS THAN (20190115) ENGINE = InnoDB,
 PARTITION p20190115 VALUES LESS THAN (20190116) ENGINE = InnoDB,
 PARTITION p20190116 VALUES LESS THAN (20190117) ENGINE = InnoDB,
 PARTITION p20190117 VALUES LESS THAN (20190118) ENGINE = InnoDB,
 PARTITION p20190118 VALUES LESS THAN (20190119) ENGINE = InnoDB,
 PARTITION p20190119 VALUES LESS THAN (20190120) ENGINE = InnoDB,
 PARTITION p20190120 VALUES LESS THAN (20190121) ENGINE = InnoDB,
 PARTITION p20190121 VALUES LESS THAN (20190122) ENGINE = InnoDB,
 PARTITION p20190122 VALUES LESS THAN (20190123) ENGINE = InnoDB,
 PARTITION p20190123 VALUES LESS THAN (20190124) ENGINE = InnoDB,
 PARTITION p20190124 VALUES LESS THAN (20190125) ENGINE = InnoDB,
 PARTITION p20190125 VALUES LESS THAN (20190126) ENGINE = InnoDB,
 PARTITION p20190126 VALUES LESS THAN (20190127) ENGINE = InnoDB,
 PARTITION p20190127 VALUES LESS THAN (20190128) ENGINE = InnoDB,
 PARTITION p20190128 VALUES LESS THAN (20190129) ENGINE = InnoDB,
 PARTITION p20190129 VALUES LESS THAN (20190130) ENGINE = InnoDB,
 PARTITION p20190130 VALUES LESS THAN (20190131) ENGINE = InnoDB,
 PARTITION p20190131 VALUES LESS THAN (20190201) ENGINE = InnoDB,
 PARTITION p20190201 VALUES LESS THAN (20190202) ENGINE = InnoDB,
 PARTITION p20190202 VALUES LESS THAN (20190203) ENGINE = InnoDB,
 PARTITION p20190203 VALUES LESS THAN (20190204) ENGINE = InnoDB,
 PARTITION p20190204 VALUES LESS THAN (20190205) ENGINE = InnoDB,
 PARTITION p20190205 VALUES LESS THAN (20190206) ENGINE = InnoDB,
 PARTITION p20190206 VALUES LESS THAN (20190207) ENGINE = InnoDB,
 PARTITION p20190207 VALUES LESS THAN (20190208) ENGINE = InnoDB,
 PARTITION p20190208 VALUES LESS THAN (20190209) ENGINE = InnoDB,
 PARTITION p20190209 VALUES LESS THAN (20190210) ENGINE = InnoDB,
 PARTITION p20190210 VALUES LESS THAN (20190211) ENGINE = InnoDB,
 PARTITION p20190211 VALUES LESS THAN (20190212) ENGINE = InnoDB,
 PARTITION p20190212 VALUES LESS THAN (20190213) ENGINE = InnoDB,
 PARTITION p20190213 VALUES LESS THAN (20190214) ENGINE = InnoDB,
 PARTITION p20190214 VALUES LESS THAN (20190215) ENGINE = InnoDB,
 PARTITION p20190215 VALUES LESS THAN (20190216) ENGINE = InnoDB,
 PARTITION p20190216 VALUES LESS THAN (20190217) ENGINE = InnoDB,
 PARTITION p20190217 VALUES LESS THAN (20190218) ENGINE = InnoDB,
 PARTITION p20190218 VALUES LESS THAN (20190219) ENGINE = InnoDB,
 PARTITION p20190219 VALUES LESS THAN (20190220) ENGINE = InnoDB) */;

/*Table structure for table `tb_cashier_trade_order201902` */

DROP TABLE IF EXISTS `tb_cashier_trade_order201902`;

CREATE TABLE `tb_cashier_trade_order201902` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `order` varchar(128) NOT NULL DEFAULT '',
  `user_code` varchar(50) NOT NULL DEFAULT '',
  `name` varchar(50) NOT NULL DEFAULT '',
  `shop` int(11) NOT NULL DEFAULT '0',
  `stall` int(11) NOT NULL DEFAULT '0',
  `machine` int(11) NOT NULL DEFAULT '0',
  `paid` decimal(18,2) NOT NULL DEFAULT '0.00',
  `refund` decimal(18,2) NOT NULL DEFAULT '0.00',
  `status` tinyint(4) NOT NULL DEFAULT '0',
  `payer_account` varchar(50) DEFAULT '',
  `pay_amount` double DEFAULT '0',
  `alipay_order` varchar(128) NOT NULL DEFAULT '',
  `type` int(11) DEFAULT '0',
  `trade_name` varchar(50) DEFAULT '',
  `create_time` datetime NOT NULL,
  `finish_time` datetime NOT NULL,
  `operator` varchar(160) DEFAULT '',
  `terminal_number` varchar(128) DEFAULT '',
  `alipay_red` decimal(18,2) DEFAULT '0.00',
  `collection_treasure` decimal(18,2) DEFAULT '0.00',
  `alipay_discount` decimal(18,2) DEFAULT '0.00',
  `merchant_discount` decimal(18,2) DEFAULT '0.00',
  `ticket_money` decimal(18,2) DEFAULT '0.00',
  `ticket_name` varchar(50) DEFAULT '',
  `merchant_red_consumption` decimal(18,2) DEFAULT '0.00',
  `card_consumption` decimal(18,2) DEFAULT '0.00',
  `refund_batch_number` varchar(50) DEFAULT '',
  `service_charge` decimal(18,2) DEFAULT '0.00',
  `shares_profit` decimal(18,2) DEFAULT '0.00',
  `remark` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`,`finish_time`),
  KEY `id` (`id`),
  KEY `time` (`create_time`,`finish_time`),
  KEY `UQ__tb_cashier_trade__63A48FA2` (`order`),
  KEY `status` (`status`),
  KEY `type` (`type`),
  KEY `terminal_number` (`terminal_number`),
  KEY `payer_account` (`payer_account`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8
/*!50100 PARTITION BY RANGE (TO_DAYS(finish_time))
(PARTITION p201902001 VALUES LESS THAN (737456) ENGINE = InnoDB,
 PARTITION p201902002 VALUES LESS THAN (737457) ENGINE = InnoDB,
 PARTITION p201902003 VALUES LESS THAN (737458) ENGINE = InnoDB,
 PARTITION p201902004 VALUES LESS THAN (737459) ENGINE = InnoDB,
 PARTITION p201902005 VALUES LESS THAN (737460) ENGINE = InnoDB,
 PARTITION p201902006 VALUES LESS THAN (737461) ENGINE = InnoDB,
 PARTITION p201902007 VALUES LESS THAN (737462) ENGINE = InnoDB,
 PARTITION p201902008 VALUES LESS THAN (737463) ENGINE = InnoDB,
 PARTITION p201902009 VALUES LESS THAN (737464) ENGINE = InnoDB,
 PARTITION p201902010 VALUES LESS THAN (737465) ENGINE = InnoDB,
 PARTITION p201902011 VALUES LESS THAN (737466) ENGINE = InnoDB,
 PARTITION p201902012 VALUES LESS THAN (737467) ENGINE = InnoDB,
 PARTITION p201902013 VALUES LESS THAN (737468) ENGINE = InnoDB,
 PARTITION p201902014 VALUES LESS THAN (737469) ENGINE = InnoDB,
 PARTITION p201902015 VALUES LESS THAN (737470) ENGINE = InnoDB,
 PARTITION p201902016 VALUES LESS THAN (737471) ENGINE = InnoDB,
 PARTITION p201902017 VALUES LESS THAN (737472) ENGINE = InnoDB,
 PARTITION p201902018 VALUES LESS THAN (737473) ENGINE = InnoDB,
 PARTITION p201902019 VALUES LESS THAN (737474) ENGINE = InnoDB,
 PARTITION p201902020 VALUES LESS THAN (737475) ENGINE = InnoDB,
 PARTITION p201902021 VALUES LESS THAN (737476) ENGINE = InnoDB,
 PARTITION p201902022 VALUES LESS THAN (737477) ENGINE = InnoDB,
 PARTITION p201902023 VALUES LESS THAN (737478) ENGINE = InnoDB,
 PARTITION p201902024 VALUES LESS THAN (737479) ENGINE = InnoDB,
 PARTITION p201902025 VALUES LESS THAN (737480) ENGINE = InnoDB,
 PARTITION p201902026 VALUES LESS THAN (737481) ENGINE = InnoDB,
 PARTITION p201902027 VALUES LESS THAN (737482) ENGINE = InnoDB,
 PARTITION p201902028 VALUES LESS THAN (737483) ENGINE = InnoDB,
 PARTITION p201902030 VALUES LESS THAN (737484) ENGINE = InnoDB) */;

/*Table structure for table `tb_column_info_list` */

DROP TABLE IF EXISTS `tb_column_info_list`;

CREATE TABLE `tb_column_info_list` (
  `column_id` int(11) NOT NULL AUTO_INCREMENT,
  `card_add_id` int(11) DEFAULT '0',
  `code` varchar(50) DEFAULT '',
  `operate_type` varchar(50) DEFAULT '',
  `title` varchar(50) DEFAULT '',
  `value` varchar(50) DEFAULT '',
  `more_info_title` varchar(50) DEFAULT '',
  `more_info_url` varchar(500) DEFAULT '',
  `index_id` int(11) DEFAULT '0',
  PRIMARY KEY (`column_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_column_info_two_list` */

DROP TABLE IF EXISTS `tb_column_info_two_list`;

CREATE TABLE `tb_column_info_two_list` (
  `column_two_id` int(11) NOT NULL AUTO_INCREMENT,
  `card_add_id` int(11) DEFAULT '0',
  `code` varchar(50) DEFAULT '',
  `operate_type` varchar(50) DEFAULT '',
  `image_id` int(11) DEFAULT '0',
  `title` varchar(50) DEFAULT '',
  `value` varchar(50) DEFAULT '',
  `url` varchar(150) DEFAULT '',
  `index_id` int(11) DEFAULT '0',
  `lv` int(11) DEFAULT '0',
  PRIMARY KEY (`column_two_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_contractinfo` */

DROP TABLE IF EXISTS `tb_contractinfo`;

CREATE TABLE `tb_contractinfo` (
  `contractId` int(11) NOT NULL,
  `contractName` varchar(100) DEFAULT NULL,
  `ali_user_Id` varchar(100) DEFAULT NULL,
  `school_code` varchar(100) DEFAULT NULL,
  `contractTime` datetime DEFAULT NULL,
  `createTime` datetime DEFAULT NULL,
  PRIMARY KEY (`contractId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_entrance_record` */

DROP TABLE IF EXISTS `tb_entrance_record`;

CREATE TABLE `tb_entrance_record` (
  `id` bigint(20) NOT NULL DEFAULT '0',
  `user_id` int(11) DEFAULT '0',
  `device_id` varchar(12) DEFAULT '',
  `open_time` datetime DEFAULT NULL,
  `entrance_status` varchar(10) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_ibeacon` */

DROP TABLE IF EXISTS `tb_ibeacon`;

CREATE TABLE `tb_ibeacon` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `schoolCode` varchar(30) DEFAULT '',
  `uuid` varchar(100) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_luckdraw` */

DROP TABLE IF EXISTS `tb_luckdraw`;

CREATE TABLE `tb_luckdraw` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `luckdrawName` varchar(50) DEFAULT '',
  `tb_RedPacket_id` int(11) DEFAULT '0',
  `luckDrawCount` int(11) DEFAULT '0',
  `active_status` tinyint(4) DEFAULT '0',
  `activeurl` varchar(100) DEFAULT '',
  `consumptionCount` int(11) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_luckdraw_city` */

DROP TABLE IF EXISTS `tb_luckdraw_city`;

CREATE TABLE `tb_luckdraw_city` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cityname` varchar(50) DEFAULT '',
  `ali_user_id` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_luckdraw_extensionuser` */

DROP TABLE IF EXISTS `tb_luckdraw_extensionuser`;

CREATE TABLE `tb_luckdraw_extensionuser` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ali_user_id` varchar(50) DEFAULT '',
  `luckDraw_id` int(11) DEFAULT '0',
  `luckDraw_city_id` int(11) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=269 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_luckdraw_extensionuser_item` */

DROP TABLE IF EXISTS `tb_luckdraw_extensionuser_item`;

CREATE TABLE `tb_luckdraw_extensionuser_item` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `extensionUserid` int(11) DEFAULT '0',
  `ali_user_id` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4424 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_luckdraw_item` */

DROP TABLE IF EXISTS `tb_luckdraw_item`;

CREATE TABLE `tb_luckdraw_item` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tb_LuckDraw_id` int(11) DEFAULT '0',
  `ali_user_id` varchar(50) DEFAULT '',
  `pay_time` datetime DEFAULT NULL,
  `prizeid` int(11) DEFAULT '0',
  `isConvert` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9964 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_luckdraw_prize` */

DROP TABLE IF EXISTS `tb_luckdraw_prize`;

CREATE TABLE `tb_luckdraw_prize` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tb_LuckDraw_id` int(11) DEFAULT '0',
  `prizeName` varchar(50) DEFAULT '',
  `prizeCount` int(11) DEFAULT '0',
  `prizeTypes` int(11) DEFAULT '0',
  `activeURL` varchar(200) DEFAULT '',
  `prizeremark` varchar(100) DEFAULT '',
  `isDel` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_manage` */

DROP TABLE IF EXISTS `tb_manage`;

CREATE TABLE `tb_manage` (
  `manage_id` int(11) NOT NULL DEFAULT '0',
  `school_id` int(11) DEFAULT '0',
  `manage_account` varchar(50) DEFAULT '',
  `manage_password` varchar(64) DEFAULT '',
  `manage_class` int(11) DEFAULT '0',
  `manage_name` varchar(50) DEFAULT '',
  `manage_telephone` varchar(20) DEFAULT '',
  `manage_address` varchar(60) DEFAULT '',
  `manage_Bool` int(11) DEFAULT '0',
  PRIMARY KEY (`manage_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_manage_module` */

DROP TABLE IF EXISTS `tb_manage_module`;

CREATE TABLE `tb_manage_module` (
  `manage_module_id` int(11) NOT NULL AUTO_INCREMENT,
  `school_id` int(11) DEFAULT '0',
  `manage_id` int(11) DEFAULT '0',
  `module_id` int(11) DEFAULT '0',
  `permit_add` int(11) DEFAULT '0',
  `permit_del` int(11) DEFAULT '0',
  `permit_sel` int(11) DEFAULT '0',
  PRIMARY KEY (`manage_module_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_manage_user` */

DROP TABLE IF EXISTS `tb_manage_user`;

CREATE TABLE `tb_manage_user` (
  `manage_user_id` int(11) NOT NULL DEFAULT '0',
  `manage_id` int(11) DEFAULT '0',
  `user_class_id` int(11) DEFAULT '0',
  `permit_add` int(11) DEFAULT '0',
  `permit_del` int(11) DEFAULT '0',
  `permit_sel` int(11) DEFAULT '0',
  PRIMARY KEY (`manage_user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_menuinfo` */

DROP TABLE IF EXISTS `tb_menuinfo`;

CREATE TABLE `tb_menuinfo` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(50) DEFAULT '',
  `href` varchar(200) DEFAULT '',
  `class` varchar(200) DEFAULT '',
  `imgsrc` varchar(200) DEFAULT '',
  `activeImage` varchar(200) DEFAULT '',
  `index` int(11) DEFAULT '0',
  `p_id` int(11) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_module` */

DROP TABLE IF EXISTS `tb_module`;

CREATE TABLE `tb_module` (
  `Module_ID` varchar(16) NOT NULL DEFAULT '',
  `Module_name` varchar(20) DEFAULT '',
  `Module_link` varchar(100) DEFAULT '',
  `Module_logo` varchar(50) DEFAULT '',
  `Module_state` int(11) DEFAULT '0',
  `Module_level` int(11) DEFAULT '0',
  `Module_up_id` int(11) DEFAULT '0',
  `Module_remark` varchar(200) DEFAULT '',
  PRIMARY KEY (`Module_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_pay_product` */

DROP TABLE IF EXISTS `tb_pay_product`;

CREATE TABLE `tb_pay_product` (
  `product_id` int(11) NOT NULL AUTO_INCREMENT,
  `pro_name` varchar(50) DEFAULT '',
  `pay_platform` int(11) DEFAULT '0',
  `pay_channel` varchar(50) DEFAULT '',
  `is_default` int(11) DEFAULT '0',
  `merchant_id` int(11) DEFAULT '0',
  `payment_item_id` int(11) DEFAULT '0',
  `sign_id` int(11) DEFAULT '0',
  `reviewed_state` int(11) DEFAULT '0',
  `rate` double DEFAULT '0',
  `create_date` datetime DEFAULT NULL,
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_accounts` */

DROP TABLE IF EXISTS `tb_payment_accounts`;

CREATE TABLE `tb_payment_accounts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `schoolcode` varchar(50) DEFAULT '',
  `pid` varchar(100) DEFAULT '',
  `appid` varchar(100) DEFAULT '',
  `name` varchar(100) DEFAULT '',
  `private_key` text,
  `publickey` text,
  `alipay_public_key` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10068 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_alipay_record` */

DROP TABLE IF EXISTS `tb_payment_alipay_record`;

CREATE TABLE `tb_payment_alipay_record` (
  `alipay_order` varchar(50) NOT NULL DEFAULT '',
  `order` varchar(50) NOT NULL DEFAULT '',
  `type` tinyint(4) NOT NULL DEFAULT '0',
  `schoolcode` varchar(50) DEFAULT '',
  `create_time` datetime NOT NULL,
  PRIMARY KEY (`alipay_order`,`order`,`type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_ar` */

DROP TABLE IF EXISTS `tb_payment_ar`;

CREATE TABLE `tb_payment_ar` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `schoolcode` varchar(50) DEFAULT '',
  `ARID` varchar(100) DEFAULT '',
  `name` varchar(100) DEFAULT '',
  `amount` decimal(11,2) DEFAULT '0.00',
  `JSstatus` int(11) DEFAULT '0',
  `status` int(11) DEFAULT '0',
  `AR_account` varchar(100) DEFAULT '',
  `star_date` datetime DEFAULT NULL,
  `end_date` datetime DEFAULT NULL,
  `st_name` varchar(100) DEFAULT '',
  `passport` varchar(100) DEFAULT '',
  `fact_amount` decimal(11,2) DEFAULT '0.00',
  `class_name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_tb_payment_AR` (`passport`) USING BTREE,
  KEY `IX_tb_payment_AR_schoolcode` (`schoolcode`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=391193 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_ar_record` */

DROP TABLE IF EXISTS `tb_payment_ar_record`;

CREATE TABLE `tb_payment_ar_record` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ar_id` int(11) NOT NULL DEFAULT '0',
  `trade_no` varchar(50) DEFAULT '',
  `out_trade_no` varchar(50) NOT NULL DEFAULT '',
  `payer_passport` varchar(50) NOT NULL DEFAULT '',
  `pay_amount` decimal(11,2) NOT NULL DEFAULT '0.00',
  `refund_amount` decimal(11,2) NOT NULL DEFAULT '0.00',
  `status` tinyint(4) NOT NULL DEFAULT '0',
  `pay_time` datetime NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_tb_payment_ar_record` (`payer_passport`) USING BTREE,
  KEY `tb_payment_ar_recordstatus` (`status`) USING BTREE,
  KEY `tb_payment_ar_recordar_id` (`ar_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=85348 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_electricitybills` */

DROP TABLE IF EXISTS `tb_payment_electricitybills`;

CREATE TABLE `tb_payment_electricitybills` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ordernumber` varchar(50) DEFAULT '',
  `room_id` int(11) DEFAULT '0',
  `pay_amount` double DEFAULT '0',
  `pay_time` datetime DEFAULT NULL,
  `pay_status` tinyint(4) DEFAULT '0',
  `schoolcode` varchar(20) DEFAULT '',
  `appaccounts_id` int(11) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `unique_tb_payment_electricitybills` (`schoolcode`,`ordernumber`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=47044 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_item` */

DROP TABLE IF EXISTS `tb_payment_item`;

CREATE TABLE `tb_payment_item` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `schoolcode` varchar(50) DEFAULT '',
  `name` varchar(50) NOT NULL DEFAULT '',
  `is_external` int(11) NOT NULL DEFAULT '0',
  `fixed` int(11) DEFAULT '0',
  `account` varchar(100) NOT NULL DEFAULT '',
  `target` int(11) NOT NULL DEFAULT '0',
  `money` decimal(11,2) NOT NULL DEFAULT '0.00',
  `type` int(11) NOT NULL DEFAULT '0',
  `introduction` varchar(1000) NOT NULL DEFAULT '',
  `icon` int(11) NOT NULL DEFAULT '0',
  `group` int(11) NOT NULL DEFAULT '0',
  `method` int(11) NOT NULL DEFAULT '0',
  `can_set_count` tinyint(4) NOT NULL DEFAULT '0',
  `nessary_info` varchar(100) NOT NULL DEFAULT '',
  `date_from` datetime NOT NULL,
  `date_to` datetime NOT NULL,
  `count` int(11) NOT NULL DEFAULT '0',
  `notify_link` varchar(200) DEFAULT '',
  `notify_key` varchar(50) DEFAULT '',
  `notify_msg` varchar(200) DEFAULT '',
  `remark` varchar(200) DEFAULT '',
  `status` tinyint(4) NOT NULL DEFAULT '0',
  `limit` int(11) DEFAULT '0',
  `class_id` int(11) DEFAULT '0',
  `level` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=144 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_nessary_info` */

DROP TABLE IF EXISTS `tb_payment_nessary_info`;

CREATE TABLE `tb_payment_nessary_info` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_record` */

DROP TABLE IF EXISTS `tb_payment_record`;

CREATE TABLE `tb_payment_record` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `order_no` varchar(50) DEFAULT '',
  `out_order_no` varchar(50) NOT NULL DEFAULT '',
  `payment_id` int(11) NOT NULL DEFAULT '0',
  `total_amount` decimal(11,2) NOT NULL DEFAULT '0.00',
  `receipt_amount` decimal(11,2) NOT NULL DEFAULT '0.00',
  `pay_amount` decimal(11,2) NOT NULL DEFAULT '0.00',
  `refund_amount` decimal(11,2) NOT NULL DEFAULT '0.00',
  `payer_id` varchar(50) NOT NULL DEFAULT '',
  `pay_time` datetime NOT NULL,
  `status` int(11) NOT NULL DEFAULT '0',
  `student_id` varchar(50) DEFAULT '',
  `pay_name` varchar(50) DEFAULT '',
  `passport` varchar(50) DEFAULT '',
  `phone` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21813 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_record_count` */

DROP TABLE IF EXISTS `tb_payment_record_count`;

CREATE TABLE `tb_payment_record_count` (
  `id` int(11) NOT NULL DEFAULT '0',
  `count` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_sub_admin` */

DROP TABLE IF EXISTS `tb_payment_sub_admin`;

CREATE TABLE `tb_payment_sub_admin` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL DEFAULT '',
  `department` varchar(50) NOT NULL DEFAULT '',
  `is_sys` tinyint(4) NOT NULL DEFAULT '0',
  `see_id` varchar(50) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_sub_admin_permission` */

DROP TABLE IF EXISTS `tb_payment_sub_admin_permission`;

CREATE TABLE `tb_payment_sub_admin_permission` (
  `sub_admin_id` int(11) NOT NULL DEFAULT '0',
  `payment_item` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`sub_admin_id`,`payment_item`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_sub_user` */

DROP TABLE IF EXISTS `tb_payment_sub_user`;

CREATE TABLE `tb_payment_sub_user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL DEFAULT '',
  `department` varchar(50) NOT NULL DEFAULT '',
  `is_sys` tinyint(4) NOT NULL DEFAULT '0',
  `password` varchar(50) NOT NULL DEFAULT '',
  `menu` varchar(50) DEFAULT '',
  `remark` varchar(200) DEFAULT '',
  `username` varchar(50) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_thirdparty_record` */

DROP TABLE IF EXISTS `tb_payment_thirdparty_record`;

CREATE TABLE `tb_payment_thirdparty_record` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `OrderName` varchar(50) DEFAULT NULL,
  `price` decimal(10,2) DEFAULT NULL,
  `schoolcode` varchar(50) DEFAULT NULL,
  `paystate` bit(1) DEFAULT NULL,
  `out_trade_no` varchar(50) DEFAULT NULL,
  `trade_no` varchar(50) DEFAULT NULL,
  `paytime` datetime DEFAULT NULL,
  `aliuserid` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_total` */

DROP TABLE IF EXISTS `tb_payment_total`;

CREATE TABLE `tb_payment_total` (
  `date` datetime NOT NULL,
  `people` int(11) NOT NULL DEFAULT '0',
  `money` decimal(11,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`date`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_type` */

DROP TABLE IF EXISTS `tb_payment_type`;

CREATE TABLE `tb_payment_type` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `schoolcode` varchar(50) DEFAULT '',
  `name` varchar(50) NOT NULL DEFAULT '',
  `introduction` varchar(50) NOT NULL DEFAULT '',
  `create_time` datetime NOT NULL,
  `is_display` tinyint(4) NOT NULL DEFAULT '0',
  `icon` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=107 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_payment_waterrent` */

DROP TABLE IF EXISTS `tb_payment_waterrent`;

CREATE TABLE `tb_payment_waterrent` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `orderId` varchar(50) DEFAULT '',
  `appId` bigint(20) DEFAULT '0',
  `aliUserId` varchar(50) DEFAULT '',
  `userId` bigint(20) DEFAULT '0',
  `deptId` bigint(20) DEFAULT '0',
  `posPay` decimal(11,2) DEFAULT '0.00',
  `posDataTime` datetime DEFAULT NULL,
  `orderState` tinyint(4) DEFAULT '0',
  `appaccounts_id` int(11) DEFAULT '0',
  `remarks` varchar(200) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_qrcode_base_config` */

DROP TABLE IF EXISTS `tb_qrcode_base_config`;

CREATE TABLE `tb_qrcode_base_config` (
  `id` bigint(20) NOT NULL DEFAULT '0',
  `school_id` int(11) DEFAULT '0',
  `user_key` varchar(8) DEFAULT '',
  `door_limit` varchar(2) DEFAULT '',
  `door_openusetime` varchar(2) DEFAULT '',
  `door_openstyle` varchar(2) DEFAULT '',
  `door_opentime` varchar(4) DEFAULT '',
  `extend_field` varchar(6) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_redpacket` */

DROP TABLE IF EXISTS `tb_redpacket`;

CREATE TABLE `tb_redpacket` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `coupon_name` varchar(32) DEFAULT '',
  `prize_type` varchar(6) DEFAULT '',
  `total_money` decimal(11,2) DEFAULT '0.00',
  `total_num` int(11) DEFAULT '0',
  `prize_msg` varchar(64) DEFAULT '',
  `start_time` datetime DEFAULT NULL,
  `end_time` datetime DEFAULT NULL,
  `merchant_link` varchar(64) DEFAULT '',
  `send_freqency` varchar(20) DEFAULT '',
  `crowd_no` varchar(128) DEFAULT '',
  `pay_url` varchar(256) DEFAULT '',
  `origin_crowd_no` varchar(64) DEFAULT '',
  `paycount` int(11) DEFAULT '0',
  `camp_status` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_redpacket_allmid` */

DROP TABLE IF EXISTS `tb_redpacket_allmid`;

CREATE TABLE `tb_redpacket_allmid` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `mids` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_redpacket_jumpurl` */

DROP TABLE IF EXISTS `tb_redpacket_jumpurl`;

CREATE TABLE `tb_redpacket_jumpurl` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `redpacket_id` int(11) DEFAULT '0',
  `jumpUrl` varchar(100) DEFAULT '',
  `activityName` varchar(100) DEFAULT '',
  `pay_Index` int(11) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_redpacket_mid` */

DROP TABLE IF EXISTS `tb_redpacket_mid`;

CREATE TABLE `tb_redpacket_mid` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `redpacket_id` int(11) DEFAULT '0',
  `mid` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=405 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_redpacket_trigger` */

DROP TABLE IF EXISTS `tb_redpacket_trigger`;

CREATE TABLE `tb_redpacket_trigger` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` varchar(40) DEFAULT '',
  `crowd_no` varchar(128) DEFAULT '',
  `trigger_result` varchar(8) DEFAULT '',
  `prize_amount` decimal(10,2) DEFAULT '0.00',
  `repeat_trigger_flag` varchar(8) DEFAULT '',
  `partner_id` varchar(40) DEFAULT '',
  `error_msg` varchar(200) DEFAULT '',
  `coupon_name` varchar(32) DEFAULT '',
  `prize_msg` varchar(64) DEFAULT '',
  `merchant_logo` varchar(200) DEFAULT '',
  `biz_no` varchar(96) DEFAULT '',
  `out_biz_no` varchar(96) DEFAULT '',
  `pay_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9510 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_auth` */

DROP TABLE IF EXISTS `tb_school_auth`;

CREATE TABLE `tb_school_auth` (
  `id` bigint(20) NOT NULL DEFAULT '0',
  `PID` varchar(50) DEFAULT '',
  `app_auth_token` varchar(50) DEFAULT '',
  `ISV_appid` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_card_template` */

DROP TABLE IF EXISTS `tb_school_card_template`;

CREATE TABLE `tb_school_card_template` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Card_add_ID` int(11) DEFAULT '0',
  `School_ID` varchar(32) DEFAULT '',
  `request_id` varchar(32) DEFAULT '',
  `template_id` varchar(32) DEFAULT '',
  `card_type` varchar(32) DEFAULT '',
  `biz_no_prefix` varchar(10) DEFAULT '',
  `biz_no_suffix_len` varchar(2) DEFAULT '',
  `write_off_type` varchar(32) DEFAULT '',
  `card_show_name` varchar(10) DEFAULT '',
  `logo_id` varchar(100) DEFAULT '',
  `background_id` varchar(100) DEFAULT '',
  `bg_color` varchar(32) DEFAULT '',
  `front_text_list_enable` varchar(5) DEFAULT '',
  `front_image_enable` varchar(5) DEFAULT '',
  `T_template_benefit_info` text,
  `T_column_info_list` text,
  `T_field_rule_list` text,
  `T_card_action_list` text,
  `T_open_card_conf` varchar(4000) DEFAULT '',
  `T_pub_channels` varchar(1000) DEFAULT '',
  `T_card_level_conf` text,
  `T_mdcode_notify_conf` varchar(2000) DEFAULT '',
  `service_label_list` varchar(1000) DEFAULT '',
  `shop_ids` varchar(1000) DEFAULT '',
  `card_spec_tag` varchar(20) DEFAULT '',
  `card_text` varchar(500) DEFAULT '',
  `column_info_layout` varchar(20) DEFAULT '',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=164 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_card_template_column` */

DROP TABLE IF EXISTS `tb_school_card_template_column`;

CREATE TABLE `tb_school_card_template_column` (
  `ColumId` varchar(50) NOT NULL,
  `T_column_info` text,
  `School_ID` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`ColumId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_card_template_former` */

DROP TABLE IF EXISTS `tb_school_card_template_former`;

CREATE TABLE `tb_school_card_template_former` (
  `id` int(4) NOT NULL AUTO_INCREMENT,
  `Logo_id` varchar(100) DEFAULT NULL,
  `background_id` varchar(100) DEFAULT NULL,
  `T_column_info_list` text,
  `T_card_action_list` text,
  `column_info_layout` varchar(20) DEFAULT NULL,
  `schoolid` varchar(32) DEFAULT NULL,
  `baseinfoshow` bit(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_class` */

DROP TABLE IF EXISTS `tb_school_class`;

CREATE TABLE `tb_school_class` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `class_code` varchar(60) NOT NULL DEFAULT '',
  `department_id` int(11) NOT NULL DEFAULT '0',
  `name` varchar(80) DEFAULT '',
  `class_info` varchar(150) DEFAULT '',
  `schoolcode` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`),
  KEY `UQ__tb_school_class__54624C12` (`class_code`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=4986 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_classinfo` */

DROP TABLE IF EXISTS `tb_school_classinfo`;

CREATE TABLE `tb_school_classinfo` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `schoolcode` varchar(50) DEFAULT NULL COMMENT '学校编号',
  `BranchID` int(11) DEFAULT NULL COMMENT '分院ID',
  `DepartmentID` int(11) DEFAULT NULL COMMENT '系ID',
  `department_classID` int(11) DEFAULT NULL COMMENT '班级对应部门表id',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3338 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_department` */

DROP TABLE IF EXISTS `tb_school_department`;

CREATE TABLE `tb_school_department` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(80) DEFAULT '',
  `p_id` int(11) DEFAULT '0',
  `schoolcode` varchar(50) DEFAULT '',
  `treeLevel` int(11) DEFAULT '0',
  `isType` bit(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14781 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_departmentinfo` */

DROP TABLE IF EXISTS `tb_school_departmentinfo`;

CREATE TABLE `tb_school_departmentinfo` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `schoolcode` varchar(50) DEFAULT NULL COMMENT '学校编号',
  `BranchID` int(11) DEFAULT NULL COMMENT '分院ID',
  `departmentID` int(11) DEFAULT NULL COMMENT '部门ID',
  `department_treeID` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_device` */

DROP TABLE IF EXISTS `tb_school_device`;

CREATE TABLE `tb_school_device` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `school_id` varchar(50) DEFAULT '',
  `device_id` varchar(12) DEFAULT '',
  `device_name` varchar(50) DEFAULT '',
  `device_state` int(11) DEFAULT '0',
  `shop_id` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_info` */

DROP TABLE IF EXISTS `tb_school_info`;

CREATE TABLE `tb_school_info` (
  `School_Code` varchar(20) NOT NULL DEFAULT '',
  `School_name` varchar(60) DEFAULT '',
  `province` varchar(50) DEFAULT '',
  `type` int(11) DEFAULT '0',
  `pid` varchar(20) DEFAULT '',
  `app_id` varchar(20) DEFAULT '',
  `private_key` text,
  `publicKey` text,
  `alipay_public_key` text,
  `project_no` varchar(30) DEFAULT '',
  `xiyunMCode` text,
  `batch` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`School_Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_scene` */

DROP TABLE IF EXISTS `tb_school_scene`;

CREATE TABLE `tb_school_scene` (
  `scene_id` int(11) NOT NULL AUTO_INCREMENT,
  `school_id` int(11) DEFAULT '0',
  `scene_name` varchar(50) DEFAULT '',
  `scene_text` varchar(400) DEFAULT '',
  PRIMARY KEY (`scene_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_scene_mack` */

DROP TABLE IF EXISTS `tb_school_scene_mack`;

CREATE TABLE `tb_school_scene_mack` (
  `mack_id` int(11) NOT NULL AUTO_INCREMENT,
  `scene_id` int(11) DEFAULT '0',
  `mack_name` varchar(50) DEFAULT '',
  `mack_code` varchar(50) DEFAULT '',
  `mack_text` varchar(200) DEFAULT '',
  PRIMARY KEY (`mack_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_user` */

DROP TABLE IF EXISTS `tb_school_user`;

CREATE TABLE `tb_school_user` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
  `school_id` varchar(50) DEFAULT '',
  `class_id` int(11) DEFAULT '0',
  `student_id` varchar(50) DEFAULT '',
  `user_name` varchar(50) DEFAULT '',
  `passport` varchar(50) NOT NULL DEFAULT '',
  `card_add_id` int(11) DEFAULT '0',
  `card_state` int(11) DEFAULT '0',
  `profession` varchar(50) DEFAULT '',
  `card_validity` datetime DEFAULT NULL,
  `biz_card_no` varchar(50) DEFAULT '',
  `ali_user_id` varchar(50) DEFAULT '',
  `create_time` datetime DEFAULT NULL,
  `department` varchar(200) DEFAULT '',
  `gender` varchar(50) DEFAULT '',
  `cell` varchar(50) DEFAULT '',
  `nationality` varchar(50) DEFAULT '(1)',
  `building_no` varchar(50) DEFAULT '',
  `welcome_flg` tinyint(4) DEFAULT '1',
  `class_code` varchar(50) DEFAULT '(0)',
  `Collarcard_time` datetime DEFAULT NULL,
  `department_id` int(11) DEFAULT '0',
  `isMultiple` bit(1) DEFAULT NULL,
  `card_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`user_id`),
  KEY `aliuid` (`ali_user_id`) USING BTREE,
  KEY `IX_tb_school_user` (`passport`) USING BTREE,
  KEY `DF_tb_school_user_passport` (`passport`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=660985 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_user_grade` */

DROP TABLE IF EXISTS `tb_school_user_grade`;

CREATE TABLE `tb_school_user_grade` (
  `grade_id` int(11) NOT NULL AUTO_INCREMENT,
  `school_code` varchar(50) DEFAULT NULL,
  `student_id` varchar(50) DEFAULT NULL,
  `discipline` varchar(50) DEFAULT NULL,
  `examinationName` varchar(50) DEFAULT NULL,
  `term` varchar(50) DEFAULT NULL,
  `grade` double(11,1) DEFAULT NULL,
  `isQualified` bit(1) DEFAULT NULL,
  `examinationTime` datetime DEFAULT NULL,
  PRIMARY KEY (`grade_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_user_multiple` */

DROP TABLE IF EXISTS `tb_school_user_multiple`;

CREATE TABLE `tb_school_user_multiple` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `school_id` varchar(50) DEFAULT NULL,
  `class_id` int(11) DEFAULT NULL,
  `student_id` varchar(50) DEFAULT NULL,
  `user_name` varchar(50) DEFAULT NULL,
  `passport` varchar(50) DEFAULT NULL,
  `department_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_school_user_room` */

DROP TABLE IF EXISTS `tb_school_user_room`;

CREATE TABLE `tb_school_user_room` (
  `id` bigint(20) NOT NULL DEFAULT '0',
  `school_code` int(11) DEFAULT '0',
  `user_id` varchar(50) DEFAULT '',
  `floor_no` int(11) DEFAULT '0',
  `room_no` int(11) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_setting` */

DROP TABLE IF EXISTS `tb_setting`;

CREATE TABLE `tb_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `XYKURL` varchar(200) DEFAULT '',
  `ZFDTURL` varchar(200) DEFAULT '',
  `schoolcode` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_stu_prepay` */

DROP TABLE IF EXISTS `tb_stu_prepay`;

CREATE TABLE `tb_stu_prepay` (
  `prepay_id` int(11) NOT NULL AUTO_INCREMENT,
  `student_id` varchar(30) NOT NULL DEFAULT '',
  `paycode` varchar(30) DEFAULT '',
  `paystate` int(11) DEFAULT '0',
  `payInfo` text,
  `payOrder` varchar(30) DEFAULT '',
  `notify_date` datetime DEFAULT NULL,
  PRIMARY KEY (`prepay_id`)
) ENGINE=InnoDB AUTO_INCREMENT=167 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_team` */

DROP TABLE IF EXISTS `tb_team`;

CREATE TABLE `tb_team` (
  `id` bigint(20) NOT NULL DEFAULT '0',
  `teamName` varchar(100) DEFAULT '',
  `startTime` varchar(20) DEFAULT '',
  `endTime` varchar(20) DEFAULT '',
  `address` varchar(100) DEFAULT '',
  `isAddJoin` int(11) DEFAULT '0',
  `isTemporary` int(11) DEFAULT '0',
  `userid` varchar(50) DEFAULT '',
  `schoolCode` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_team_user` */

DROP TABLE IF EXISTS `tb_team_user`;

CREATE TABLE `tb_team_user` (
  `id` bigint(20) NOT NULL DEFAULT '0',
  `teamID` bigint(20) DEFAULT '0',
  `joinUserid` varchar(20) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_thirdpartyapi_config` */

DROP TABLE IF EXISTS `tb_thirdpartyapi_config`;

CREATE TABLE `tb_thirdpartyapi_config` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `schoolcode` varchar(50) DEFAULT '',
  `APIName` varchar(50) DEFAULT '',
  `APIDescribe` varchar(250) DEFAULT '',
  `APIURL` varchar(100) DEFAULT '',
  `APIType` tinyint(4) DEFAULT '0',
  `isDEL` tinyint(4) DEFAULT '0',
  `parameter` varchar(50) DEFAULT '',
  `returnResult` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_userinfo` */

DROP TABLE IF EXISTS `tb_userinfo`;

CREATE TABLE `tb_userinfo` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `schoolcode` varchar(40) DEFAULT '',
  `userName` varchar(40) DEFAULT '',
  `loginuser` varchar(40) DEFAULT '',
  `password` varchar(40) DEFAULT '',
  `roletype` int(11) DEFAULT '0',
  `dining_talls` varchar(500) DEFAULT '',
  `menus` varchar(200) DEFAULT '',
  `remark` varchar(200) DEFAULT '',
  `receivables` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=180 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_welcome_mod` */

DROP TABLE IF EXISTS `tb_welcome_mod`;

CREATE TABLE `tb_welcome_mod` (
  `School_Code` varchar(20) NOT NULL DEFAULT '',
  `class_code` varchar(60) DEFAULT '',
  `report` varchar(1000) DEFAULT '',
  `dorm` varchar(500) DEFAULT '',
  `pid` varchar(20) DEFAULT '',
  `background` varchar(60) DEFAULT '',
  `messageTo` text,
  `notice` text,
  PRIMARY KEY (`School_Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `tb_welcome_payinfo` */

DROP TABLE IF EXISTS `tb_welcome_payinfo`;

CREATE TABLE `tb_welcome_payinfo` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `paycode` varchar(20) NOT NULL DEFAULT '',
  `schoolcode` varchar(50) DEFAULT '',
  `typecode` varchar(50) NOT NULL DEFAULT '',
  `is_external` int(11) NOT NULL DEFAULT '0',
  `account` varchar(100) NOT NULL DEFAULT '',
  `target` int(11) NOT NULL DEFAULT '0',
  `money` double NOT NULL DEFAULT '0',
  `introduction` varchar(1000) DEFAULT '',
  `icon` int(11) DEFAULT '0',
  `group` int(11) DEFAULT '0',
  `method` int(11) DEFAULT '0',
  `can_set_count` tinyint(4) DEFAULT '0',
  `payname` varchar(100) DEFAULT '',
  `date_limit` datetime DEFAULT NULL,
  `remark` varchar(200) DEFAULT '',
  `status` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `UQ__tb_welcome_payIn__5556704B` (`paycode`) USING BTREE,
  KEY `UQ__tb_welcome_payIn__573EB8BD` (`paycode`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_welcome_paylist` */

DROP TABLE IF EXISTS `tb_welcome_paylist`;

CREATE TABLE `tb_welcome_paylist` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `typecode` varchar(20) NOT NULL DEFAULT '',
  `type` varchar(50) NOT NULL DEFAULT '',
  `create_time` datetime NOT NULL,
  `is_display` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `UQ__tb_welcome_payLi__5A1B2568` (`typecode`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

/*Table structure for table `tb_xiyun_notify` */

DROP TABLE IF EXISTS `tb_xiyun_notify`;

CREATE TABLE `tb_xiyun_notify` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `notify_time` varchar(20) DEFAULT '',
  `sign_type` varchar(20) DEFAULT '',
  `charset` varchar(20) DEFAULT '',
  `notify_type` varchar(20) DEFAULT '',
  `notify_id` varchar(64) DEFAULT '',
  `appid` varchar(40) DEFAULT '',
  `merchantCode` varchar(20) DEFAULT '',
  `bizId` varchar(128) DEFAULT '',
  `tradeType` int(11) DEFAULT '0',
  `payType` int(11) DEFAULT '0',
  `tradeNum` varchar(128) NOT NULL DEFAULT '',
  `orderNum` varchar(128) DEFAULT '',
  `tradeFinishedTime` datetime DEFAULT NULL,
  `receivableAmount` double DEFAULT '0',
  `extendedAmount` double DEFAULT '0',
  `Amount` double DEFAULT '0',
  `thirdUserId` varchar(40) DEFAULT '',
  `consumerCode` varchar(80) DEFAULT '',
  `consumerName` varchar(40) DEFAULT '',
  `canteenId` varchar(40) DEFAULT '',
  `canteenName` varchar(200) DEFAULT '',
  `cashierUserName` varchar(20) DEFAULT '',
  `deviceSn` varchar(128) DEFAULT '',
  `refundBizNo` varchar(64) DEFAULT '',
  `refundReason` varchar(1000) DEFAULT '',
  PRIMARY KEY (`ID`),
  KEY `deviceSn` (`deviceSn`) USING BTREE,
  KEY `ID` (`ID`) USING BTREE,
  KEY `merchantCode` (`merchantCode`) USING BTREE,
  KEY `thirdUserId` (`thirdUserId`) USING BTREE,
  KEY `tradeFinishedTime` (`tradeFinishedTime`) USING BTREE,
  KEY `UQ__tb_xiyun_notify2__5CF79213` (`tradeNum`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `website_userorder` */

DROP TABLE IF EXISTS `website_userorder`;

CREATE TABLE `website_userorder` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `schoolname` varchar(80) DEFAULT '',
  `name` varchar(80) DEFAULT '',
  `duty` varchar(80) DEFAULT '',
  `tel` varchar(20) DEFAULT '',
  `email` varchar(80) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;

/*!50106 set global event_scheduler = 1*/;

/* Event structure for event `daily_generate_partition` */

/*!50106 DROP EVENT IF EXISTS `daily_generate_partition`*/;

DELIMITER $$

/*!50106 CREATE DEFINER=`newxiaoyuan`@`%` EVENT `daily_generate_partition` ON SCHEDULE EVERY 1 HOUR STARTS '2019-01-12 16:20:00' ON COMPLETION PRESERVE ENABLE COMMENT 'Creating partitions' DO BEGIN
    #调用刚才创建的存储过程，第一个参数是数据库名称，第二个参数是表名称
    CALL create_partition_by_day('alischoolcard','tb_cashier_trade_order');
    CALL create_partition_by_day('alischoolcard','tb_xiyun_notify');
END */$$
DELIMITER ;

/* Procedure structure for procedure `create_partition_by_day` */

/*!50003 DROP PROCEDURE IF EXISTS  `create_partition_by_day` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`newxiaoyuan`@`%` PROCEDURE `create_partition_by_day`(IN_SCHEMANAME VARCHAR(64), IN_TABLENAME VARCHAR(64))
BEGIN
    #当前日期存在的分区的个数
    DECLARE ROWS_CNT INT UNSIGNED;
    #目前日期，为当前日期的后一天
    DECLARE TARGET_DATE TIMESTAMP;
    #分区的名称，格式为p20180620
    DECLARE PARTITIONNAME VARCHAR(9);
    #当前分区名称的分区值上限，即为 PARTITIONNAME + 1
    DECLARE PARTITION_ADD_DAY VARCHAR(9);
    SET TARGET_DATE = NOW() + INTERVAL 1 DAY;
    SET PARTITIONNAME = DATE_FORMAT( TARGET_DATE, 'p%Y%m%d' );
    SET TARGET_DATE = TARGET_DATE + INTERVAL 1 DAY;
    SET PARTITION_ADD_DAY = DATE_FORMAT( TARGET_DATE, '%Y%m%d' );
    SELECT COUNT(*) INTO ROWS_CNT FROM information_schema.partitions
    WHERE table_schema = IN_SCHEMANAME AND table_name = IN_TABLENAME AND partition_name = PARTITIONNAME;
    IF ROWS_CNT = 0 THEN
        SET @SQL = CONCAT( 'ALTER TABLE `', IN_SCHEMANAME, '`.`', IN_TABLENAME, '`',
        ' ADD PARTITION (PARTITION ', PARTITIONNAME, " VALUES LESS THAN (",
            PARTITION_ADD_DAY ,") ENGINE = InnoDB);" );
        PREPARE STMT FROM @SQL;
        EXECUTE STMT;
        DEALLOCATE PREPARE STMT;
     ELSE
       SELECT CONCAT("partition `", PARTITIONNAME, "` for table `",IN_SCHEMANAME, ".", IN_TABLENAME, "` already exists") AS result;
     END IF;
END */$$
DELIMITER ;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
