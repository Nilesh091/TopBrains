CREATE TABLE Students
(
  id int,
  name varchar(255),
  department varchar(255)
)
CREATE table hostel
(
  id int,
  student_id int,
  room_number int
)

CREATE TABLE Libreries
(
  id int,
  student_id int,
  book_name varchar(255)
)

--seed default data
INSERT INTO Students
  (id, name, department)
VALUES
  (1, 'Ravi', 'Mtech');
INSERT INTO Students
  (id, name, department)
VALUES
  (2, 'Mari', 'MCA');
INSERT INTO Students
  (id, name, department)
VALUES
  (3, 'Nr', 'Physics');

INSERT INTO hostel
  (id, student_id, room_number)
VALUES
  (1, 1, 101);
INSERT INTO hostel
  (id, student_id, room_number)
VALUES
  (2, 2,
    102);
INSERT INTO hostel
  (id, student_id, room_number)
VALUES
  (3, 3, 103);

INSERT INTO Libreries
  (id, student_id, book_name)
VALUES
  (1, 1, 'C#');
INSERT INTO Libreries
  (id, student_id, book_name)
VALUES
  (2, 2, 'Java');
INSERT INTO Libreries
  (id, student_id, book_name)
VALUES
  (3, 3, 'Python');


SELECT h.room_number
from hostel h
  JOIN Students s ON h.student_id = s.id
where s.name='Ravi';

--who took the java book
SELECT s.name
from Students s
  JOIN Libreries l ON s.id = l.student_id
where l.book_name='Java';

-- how many students took c# book
SELECT count(book_name)
from Libreries
where book_name='C#';

select l.book_name
from Libreries l join hostel h on l.student_id = h.student_id
where h.room_number=101;


-- join these three tables
SELECT s.name, h.room_number, l.book_name
from Students s
  JOIN hostel h ON s.id = h.student_id
  JOIN Libreries l ON s.id = l.student_id;