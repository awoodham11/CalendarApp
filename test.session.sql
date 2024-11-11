-- @block
CREATE TABLE Events(
    Event_id int primary key auto_increment,
    Title varchar(50),
    EventDate varchar(50),
    Description varchar(150)
);
-- @block
SELECT *
FROM events;
-- @block
ALTER TABLE events DROP COLUMN EventDate;
-- @block
ALTER TABLE events
ADD StartTime datetime;
ALTER TABLE events
ADD EndTime datetime;