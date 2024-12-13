CREATE DATABASE IF NOT EXISTS PhoneBookDb;

USE PhoneBookDb;

CREATE TABLE IF NOT EXISTS Contacts (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(14) NOT NULL UNIQUE,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Status INT NOT NULL
);

INSERT INTO Contacts (Name, PhoneNumber, Email, Status) VALUES ('Joao Silva', '5581912345678', 'joao.silva@email.com', 1), ('Marta Oliveira', '5581923456789', 'marta.oliveira@email.com', 1), ('Pedro Santos', '5581934567890', 'pedro.santos@email.com', 1), ('Ana Costa', '5581945678901', 'ana.costa@email.com', 1), ('Lucas Almeida', '5581956789012', 'lucas.almeida@email.com', 1), ('Fernanda Lima', '5581967890123', 'fernanda.lima@email.com', 1), ('Roberto Pereira', '5581978901234', 'roberto.pereira@email.com', 1), ('Juliana Ribeiro', '5581989012345', 'juliana.ribeiro@email.com', 1),
('Carlos Souza', '5581990123456', 'carlos.souza@email.com', 1), ('Patricia Mendes', '5581901234567', 'patricia.mendes@email.com', 1);

