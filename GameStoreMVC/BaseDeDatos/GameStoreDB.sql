USE GameStoreDB;
GO

-- tabla usuario
CREATE TABLE tb_usuario (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nombre_completo VARCHAR(100) NOT NULL,
    correo VARCHAR(100) NOT NULL UNIQUE,
    clave VARCHAR(100) NOT NULL,
    telefono VARCHAR(15),
    es_admin BIT DEFAULT 0 -- 1 si es admin, 0 si es cliente
);

-- tabla categoria
CREATE TABLE tb_categoria (
    id_categoria INT IDENTITY(1,1) PRIMARY KEY,
    descripcion VARCHAR(50) NOT NULL
);

-- tabla producto
CREATE TABLE tb_producto (
    id_producto INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(500),
    precio DECIMAL(10,2) NOT NULL,
    stock INT NOT NULL,
    imagen_url VARCHAR(255),
    id_categoria INT NOT NULL,
    FOREIGN KEY (id_categoria) REFERENCES tb_categoria(id_categoria)
);

-- tabla de pedido cabecera (la factura)
CREATE TABLE tb_pedido (
    id_pedido INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    fecha_pedido DATETIME DEFAULT GETDATE(),
    monto_total DECIMAL(10,2) NOT NULL,
    estado VARCHAR(20) DEFAULT 'Pendiente', -- pendiente o entregado
    FOREIGN KEY (id_usuario) REFERENCES tb_usuario(id_usuario)
);

-- tabla de pedido detalle
CREATE TABLE tb_detalle_pedido (
    id_detalle INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (id_pedido) REFERENCES tb_pedido(id_pedido),
    FOREIGN KEY (id_producto) REFERENCES tb_producto(id_producto)
);

-- datos de prueba
-- insertar categorias
INSERT INTO tb_categoria (descripcion) VALUES ('Videojuegos PS5');
INSERT INTO tb_categoria (descripcion) VALUES ('Videojuegos Switch');
INSERT INTO tb_categoria (descripcion) VALUES ('Consolas');
INSERT INTO tb_categoria (descripcion) VALUES ('Accesorios');

-- insertar productos
-- nota: las urls son para las imagenes
INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria) 
VALUES ('EA Sports FC 26', 'El juego de fútbol más popular del mundo.', 229.00, 6, 'https://media.falabella.com/falabellaPE/120426163_01/w=1500,h=1500,fit=pad', 1);

INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria) 
VALUES ('Super Mario Bros. Wonder', 'La nueva aventura de Mario en 2D.', 199.00, 10, 'https://media.falabella.com/falabellaPE/125259955_01/w=1500,h=1500,fit=pad', 2);

INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria) 
VALUES ('PlayStation 5 Slim', 'Consola PS5 versión estándar con lector.', 2499.00, 5, 'https://media.falabella.com/falabellaPE/137340461_01/w=800,h=800,fit=pad', 3);

INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria) 
VALUES ('Mando DualSense White', 'Control inalámbrico para PS5.', 269.00, 15, 'https://coolboxpe.vtexassets.com/arquivos/ids/179118-800-800?v=638810280116930000&width=800&height=800&aspect=true', 4);

-- 2 Accesorios
INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria) 
VALUES ('Teclado Mecánico Redragon Kumara', 'Teclado mecánico RGB para gaming.', 189.00, 8, 'https://www.necdigitalstore.com/files/images/productos/1702565554-teclado-redragon-kumara-k552-mecanico-sw-blue-rgb-negro-0.webp', 4);

INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria) 
VALUES ('Mouse Gamer Logitech G203', 'Mouse gamer RGB de alta precisión.', 109.00, 20, 'https://coolboxpe.vtexassets.com/arquivos/ids/238587-800-800?v=638978673883370000&width=800&height=800&aspect=true', 4);

-- 2 juegos ps5
INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria) 
VALUES ('Gran Turismo 7', 'Simulador de carreras exclusivo de PS5.', 209.00, 7, 'https://hiraoka.com.pe/media/catalog/product/p/s/ps5_gt7_ste_pksht_lt_rgb_la_211220.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=560&width=700&canvas=700:560', 1);

INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria) 
VALUES ('Spider-Man 2', 'Aventura de acción con Peter Parker y Miles Morales.', 239.00, 12, 'https://media.falabella.com/falabellaPE/125259841_01/w=1500,h=1500,fit=pad', 1);

-- 2 juegos de switch
INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria) 
VALUES ('Animal Crossing: New Horizons', 'Simulador de vida en una isla.', 209.00, 15, 'https://media.falabella.com/falabellaPE/113895639_01/w=1500,h=1500,fit=pad', 2);

INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria) 
VALUES ('Super Smash Bros. Ultimate', 'Peleas con los personajes de Nintendo.', 189.00, 8, 'https://media.falabella.com/falabellaPE/120426051_01/w=1500,h=1500,fit=pad', 2);

-- insertar un usuario de prueba (admin)
INSERT INTO tb_usuario (nombre_completo, correo, clave, es_admin)
VALUES ('Admin', 'admin@gamestore.com', '123456', 1);