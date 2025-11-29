CREATE DATABASE IF NOT EXISTS app_combustivel;
USE app_combustivel;

CREATE TABLE USUARIO (
    id_usuario INT NOT NULL AUTO_INCREMENT,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    senha CHAR(60) NOT NULL,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    PRIMARY KEY (id_usuario)
);

CREATE TABLE POSTO_DE_COMBUSTIVEL (
    id_posto INT NOT NULL AUTO_INCREMENT,
    nome VARCHAR(100) NOT NULL,
    cnpj VARCHAR(14) NOT NULL UNIQUE,
    endereco VARCHAR(255) NOT NULL,
    latitude DECIMAL(10, 8),
    longitude DECIMAL(11, 8),
    avaliacao_media DECIMAL(2, 1) DEFAULT 0.0,
    
    PRIMARY KEY (id_posto)
);

CREATE TABLE COMBUSTIVEL (
    id_combustivel INT NOT NULL AUTO_INCREMENT,
    tipo VARCHAR(50) NOT NULL UNIQUE,
    
    PRIMARY KEY (id_combustivel)
);

CREATE TABLE PRECO (
    id_posto INT NOT NULL,
    id_combustivel INT NOT NULL,
    valor DECIMAL(5, 2) NOT NULL,
    data_registro DATETIME NOT NULL,
    
    PRIMARY KEY (id_posto, id_combustivel, data_registro),
    
    FOREIGN KEY (id_posto) REFERENCES POSTO_DE_COMBUSTIVEL(id_posto),
    FOREIGN KEY (id_combustivel) REFERENCES COMBUSTIVEL(id_combustivel)
);

CREATE TABLE AVALIACAO (
    id_avaliacao INT NOT NULL AUTO_INCREMENT,
    id_usuario INT NOT NULL,
    id_posto INT NOT NULL,
    nota DECIMAL(2, 1) NOT NULL CHECK (nota >= 0 AND nota <= 5), -- Nota de 0 a 5
    data_avaliacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    PRIMARY KEY (id_avaliacao),
    
    FOREIGN KEY (id_usuario) REFERENCES USUARIO(id_usuario),
    FOREIGN KEY (id_posto) REFERENCES POSTO_DE_COMBUSTIVEL(id_posto),
    UNIQUE (id_usuario, id_posto) 
);

CREATE TABLE COMENTARIO (
    id_comentario INT NOT NULL AUTO_INCREMENT,
    id_usuario INT NOT NULL,
    id_posto INT NOT NULL,
    texto TEXT NOT NULL,
    data_comentario TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    PRIMARY KEY (id_comentario),
    
    FOREIGN KEY (id_usuario) REFERENCES USUARIO(id_usuario),
    FOREIGN KEY (id_posto) REFERENCES POSTO_DE_COMBUSTIVEL(id_posto)
);