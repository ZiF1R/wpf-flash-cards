use FlashCards;

-- GET cards from specific folder of user
GO
DECLARE
	@folderID int = 2,
	@userID int = 1;

SELECT
	CARDS.TERM,
	CARDS.TRANSLATION,
	CARDS.EXAMPLES,
	CARDS.CREATED_DATE
FROM USERS
JOIN FOLDERS ON USERS.UNIQUE_ID = FOLDERS.USER_UID AND FOLDERS.USER_UID = @userID
JOIN CARDS ON FOLDERS.FOLDER_ID = CARDS.FOLDER_ID AND CARDS.FOLDER_ID = @folderID;

-- GET user settings
GO
DECLARE @userID int = 1;

SELECT
	THEMES.THEME,
	LANGS.LANG,
	SETTINGS.CARDS_LIMIT,
	SETTINGS.SWITCHED_REVIEW,
	SETTINGS.TIME_LIMIT
FROM USERS
JOIN SETTINGS ON USERS.UNIQUE_ID = SETTINGS.USER_UID AND SETTINGS.USER_UID = @userID
JOIN THEMES ON SETTINGS.ACTIVE_THEME = THEMES.THEME_ID
JOIN LANGS ON SETTINGS.ACTIVE_LANG = LANGS.LANG_ID;

-- GET user folders
GO
DECLARE	@userID int = 1;

SELECT
	FOLDERS.FOLDER_ID,
	FOLDERS.FOLDER_NAME,
	CATEGORIES.CATEGORY,
	FOLDERS.CREATED_DATE,
	FOLDERS.MEMORIZED_CARDS
FROM USERS
JOIN FOLDERS ON USERS.UNIQUE_ID = FOLDERS.USER_UID AND USERS.UNIQUE_ID = @userID
JOIN CATEGORIES ON FOLDERS.CATEGORY = CATEGORIES.CATEGORY_ID;

-- GET user categories
GO
DECLARE	@userID int = 1;

SELECT
	CATEGORIES.CATEGORY_ID,
	CATEGORIES.CATEGORY
FROM USERS
JOIN CATEGORIES ON USERS.UNIQUE_ID = CATEGORIES.USER_UID AND USERS.UNIQUE_ID = @userID;