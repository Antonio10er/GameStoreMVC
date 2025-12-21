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
    formato VARCHAR(20) DEFAULT 'Físico',
    edicion VARCHAR(50) DEFAULT 'Estándar',
    id_categoria INT NOT NULL,
    activo BIT DEFAULT 1,
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

-- insertar categorias
INSERT INTO tb_categoria (descripcion) VALUES ('Videojuegos PS5');
INSERT INTO tb_categoria (descripcion) VALUES ('Videojuegos Switch');
INSERT INTO tb_categoria (descripcion) VALUES ('Consolas');
INSERT INTO tb_categoria (descripcion) VALUES ('Accesorios');

-- insertar productos
-- 1. Juegos PS5
INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria, formato, edicion) VALUES
('God of War Ragnarök', 'Kratos y Atreus enfrentan el Ragnarök en esta épica aventura nórdica.', 159.00, 7, 'https://storegames.com.pe/wp-content/uploads/2025/08/ST003-GOD.webp', 1, 'Físico', 'Estándar'),
('Marvel Spider-Man 2', 'Aventura de acción con Peter Parker y Miles Morales.', 189.00, 12, 'https://storegames.com.pe/wp-content/uploads/2023/10/SPIDERMAN2-01-1.webp', 1, 'Digital', 'Estándar'),
('The Last of Us Part I', 'La versión remake del clásico juego postapocalíptico.', 179.00, 5, 'https://storegames.com.pe/wp-content/uploads/2025/04/ST050424-18.webp', 1, 'Digital', 'Estándar'),
('EA Sports FC 26', 'El juego de fútbol más popular del mundo con tecnología HyperMotionV.', 159.00, 6, 'https://storegames.com.pe/wp-content/uploads/2025/09/ST001-FC26-1.webp', 1, 'Físico', 'Estándar'),
('Gran Turismo 7', 'Simulador de carreras exclusivo de PS5.', 165.00, 7, 'https://storegames.com.pe/wp-content/uploads/2025/04/ST050424-17.webp', 1, 'Digital', 'Estándar');


-- 2. Juegos Switch
INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria, formato, edicion) VALUES
('The Legend of Zelda: Tears of the Kingdom', 'La nueva aventura épica de Link en Hyrule.', 263.00, 6, 'https://realplaza.vtexassets.com/arquivos/ids/32122599-1200-auto?v=638132173197500000&width=1200&height=auto&aspect=true', 2, 'Digital', 'Estándar'),
('Mario Kart 8 Deluxe', 'Carreras frenéticas con los personajes de Nintendo.', 159.00, 12, 'https://realplaza.vtexassets.com/arquivos/ids/32275165-1200-auto?v=638151808263970000&width=1200&height=auto&aspect=true', 2, 'Físico', 'Estándar'),
('Animal Crossing: New Horizons', 'Crea tu propia isla y vive a tu ritmo.', 179.00, 10, 'https://realplaza.vtexassets.com/arquivos/ids/32275042-1200-auto?v=638151806260430000&width=1200&height=auto&aspect=true', 2, 'Físico', 'Estándar'),
('Pokémon Scarlet', 'Explora la región de Paldea y captura nuevos Pokémon.', 209.00, 8, 'https://realplaza.vtexassets.com/arquivos/ids/32275189-1200-auto?v=638151808848330000&width=1200&height=auto&aspect=true', 2, 'Digital', 'Estándar'),
('Super Smash Bros. Ultimate', 'El crossover definitivo de peleas de Nintendo.', 129.00, 9, 'https://realplaza.vtexassets.com/arquivos/ids/15343837-1200-auto?v=637401310041200000&width=1200&height=auto&aspect=true', 2, 'Físico', 'Estándar');

-- 3. Consolas
INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria, formato, edicion) VALUES
('PlayStation 5 Digital Slim 1TB + Astro Bot + Gran Turismo 7', 'Consola PS5 sin lector de discos.', 2299.00, 4, 'https://realplaza.vtexassets.com/arquivos/ids/38389593-1200-auto?v=638977965609470000&width=1200&height=auto&aspect=true', 3, 'Físico', 'Estándar'),
('Nintendo Switch OLED', 'Consola híbrida con pantalla OLED de 7 pulgadas.', 1599.00, 6, 'https://realplaza.vtexassets.com/arquivos/ids/32200557-1200-auto?v=638775593465100000&width=1200&height=auto&aspect=true', 3, 'Físico', 'Estándar'),
('Nintendo Switch Lite', 'Versión portátil y compacta de Nintendo Switch.', 999.00, 7, 'https://topgamesperu.com/wp-content/uploads/2025/07/Lite-Amamrillo-A.jpg', 3, 'Físico', 'Estándar'),
('Xbox Series X Negro 1TB', 'La consola más potente de Microsoft.', 2499.00, 3, 'https://realplaza.vtexassets.com/arquivos/ids/15945201-1200-auto?v=637486818893730000&width=1200&height=auto&aspect=true', 3, 'Físico', 'Estándar'),
('Xbox Series S 512GB SSD', 'Consola digital compacta y económica.', 1499.00, 5, 'https://realplaza.vtexassets.com/arquivos/ids/35296972-1200-auto?v=638464079717600000&width=1200&height=auto&aspect=true', 3, 'Físico', 'Estándar');

-- 4. Accesorios
INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria, formato, edicion) VALUES
('Mando DualSense White', 'Control inalámbrico para PS5 con retroalimentación háptica.', 269.00, 15, 'https://realplaza.vtexassets.com/arquivos/ids/37374482-1200-auto?v=638858253392830000&width=1200&height=auto&aspect=true', 4, 'Físico', 'Estándar'),
('Teclado Mecánico Redragon Kumara', 'Teclado mecánico RGB para gaming.', 189.00, 8, 'https://www.necdigitalstore.com/files/images/productos/1702565554-teclado-redragon-kumara-k552-mecanico-sw-blue-rgb-negro-0.webp', 4, 'Físico', 'Estándar'),
('Mouse Gamer Logitech G203', 'Mouse gamer RGB de alta precisión.', 109.00, 20, 'https://coolboxpe.vtexassets.com/arquivos/ids/238587-800-800?v=638978673883370000&width=800&height=800&aspect=true', 4, 'Físico', 'Estándar'),
('Pro Controller Switch', 'Control profesional para Nintendo Switch.', 329.00, 7, 'https://realplaza.vtexassets.com/arquivos/ids/36556050-1200-auto?v=638696394529630000&width=1200&height=auto&aspect=true', 4, 'Físico', 'Estándar'),
('Audifono Pro X 2 Lightspeed Wireless Blanco', 'Los audífonos con micrófono PRO X 2 LIGHTSPEED ofrecen sonido profesional', 825.00, 2, 'https://storegames.com.pe/wp-content/uploads/2025/06/ST-AGPROX-01.webp', 4, 'Físico', 'Estándar');

-- insertar un usuario de prueba (admin)
INSERT INTO tb_usuario (nombre_completo, correo, clave, es_admin)
VALUES ('Admin', 'admin@gamestore.com', '123', 1);