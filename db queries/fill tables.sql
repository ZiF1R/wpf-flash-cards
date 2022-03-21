use FlashCards;

INSERT INTO LANGS VALUES ('EN'), ('RU');
INSERT INTO THEMES VALUES ('LIGHT'), ('DARK');

INSERT INTO USERS VALUES
	('ZiF1R', 'Dobriyan', 'Alex', 'do-alex03@mail.ru', '123456qw'),
	('Ren4l', 'Isakov', 'Vladislav', 'i-vlad@mail.ru', '123456qw');

INSERT INTO SETTINGS(USER_UID, ACTIVE_THEME, ACTIVE_LANG) VALUES
	(1, 1, 1),
	(2, 1, 1);

INSERT INTO CATEGORIES VALUES
	(1, 'test1'),
	(2, 'test1');

INSERT INTO FOLDERS VALUES
	(1, 'Test folder', '03-21-22 16:00:00', 1, 0),
	(1, 'Test folder1', '03-21-22 16:00:00', 1, 0);

INSERT INTO CARDS VALUES
	(1, '03-21-22 16:10:00', 'test1', 'тест1', NULL),
	(1, '03-21-22 16:20:00', 'test2', 'тест2', 'example for test2'),
	(2, '03-21-22 16:10:00', 'test1', 'тест1', NULL),
	(2, '03-21-22 16:20:00', 'test2', 'тест2', 'example for test2');