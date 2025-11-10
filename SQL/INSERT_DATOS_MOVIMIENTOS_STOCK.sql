-- =============================================
-- Script para insertar datos de prueba
-- Movimientos de Stock (Compras y Ventas)
-- =============================================
USE [BD_INKACAO]
GO

-- =============================================
-- 1. INSERTAR PRODUCTOS (si no existen)
-- =============================================
IF NOT EXISTS (SELECT 1 FROM producto_mae WHERE nombre_producto = 'Laptop HP Core i5')
BEGIN
    INSERT INTO producto_mae (nombre_producto, stock, precio_entrada, precio_salida, estado, aud_es_eli_b)
    VALUES 
    ('Laptop HP Core i5', 50, 2500.00, 3200.00, 1, 0),
    ('Mouse Logitech M185', 150, 35.00, 55.00, 1, 0),
    ('Teclado Mecánico RGB', 80, 180.00, 280.00, 1, 0),
    ('Monitor LG 24"', 30, 550.00, 750.00, 1, 0),
    ('Impresora HP LaserJet', 25, 800.00, 1100.00, 1, 0),
    ('Disco Duro Externo 1TB', 60, 180.00, 250.00, 1, 0),
    ('Memoria RAM DDR4 8GB', 100, 120.00, 180.00, 1, 0),
    ('SSD Kingston 480GB', 45, 200.00, 290.00, 1, 0),
    ('Webcam Logitech C920', 40, 280.00, 380.00, 1, 0),
    ('Audífonos Sony WH-1000XM4', 20, 950.00, 1300.00, 1, 0);
END

-- =============================================
-- 2. INSERTAR COMPRAS (Movimientos de entrada)
-- =============================================
DECLARE @FechaBase DATETIME = DATEADD(MONTH, -3, GETDATE());

-- Compra 1: Hace 3 meses
INSERT INTO compra (tipo_documento_id, ruc, razon_social, observacion, tipo_compra_id, total, fecha_registro, 
                    aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f, 
                    aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b)
VALUES (6, '20567890123', 'DISTRIBUIDORA TECH SAC', 'Compra de equipos de cómputo', 1, 15500.00, @FechaBase,
        1, GETDATE(), 1, GETDATE(), '192.168.1.100', 'PC-ADMIN', 0);

DECLARE @CompraId1 INT = SCOPE_IDENTITY();

-- Detalles de Compra 1
INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId1, pk_prod_id, 5, precio_salida, precio_entrada, (5 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Laptop HP Core i5';

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId1, pk_prod_id, 30, precio_salida, precio_entrada, (30 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Mouse Logitech M185';

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId1, pk_prod_id, 15, precio_salida, precio_entrada, (15 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Teclado Mecánico RGB';

-- Compra 2: Hace 2 meses y medio
SET @FechaBase = DATEADD(DAY, -75, GETDATE());

INSERT INTO compra (tipo_documento_id, ruc, razon_social, observacion, tipo_compra_id, total, fecha_registro, 
                    aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f, 
                    aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b)
VALUES (6, '20876543210', 'IMPORTACIONES MONITOR PERU', 'Compra de monitores y periféricos', 1, 22000.00, @FechaBase,
        1, GETDATE(), 1, GETDATE(), '192.168.1.100', 'PC-ADMIN', 0);

DECLARE @CompraId2 INT = SCOPE_IDENTITY();

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId2, pk_prod_id, 10, precio_salida, precio_entrada, (10 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Monitor LG 24"';

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId2, pk_prod_id, 20, precio_salida, precio_entrada, (20 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Disco Duro Externo 1TB';

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId2, pk_prod_id, 25, precio_salida, precio_entrada, (25 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Memoria RAM DDR4 8GB';

-- Compra 3: Hace 2 meses
SET @FechaBase = DATEADD(DAY, -60, GETDATE());

INSERT INTO compra (tipo_documento_id, ruc, razon_social, observacion, tipo_compra_id, total, fecha_registro, 
                    aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f, 
                    aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b)
VALUES (6, '20123456789', 'TECH SUPPLIERS EIRL', 'Compra de almacenamiento', 1, 12000.00, @FechaBase,
        1, GETDATE(), 1, GETDATE(), '192.168.1.100', 'PC-ADMIN', 0);

DECLARE @CompraId3 INT = SCOPE_IDENTITY();

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId3, pk_prod_id, 20, precio_salida, precio_entrada, (20 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'SSD Kingston 480GB';

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId3, pk_prod_id, 5, precio_salida, precio_entrada, (5 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Impresora HP LaserJet';

-- Compra 4: Hace 1 mes
SET @FechaBase = DATEADD(DAY, -30, GETDATE());

INSERT INTO compra (tipo_documento_id, ruc, razon_social, observacion, tipo_compra_id, total, fecha_registro, 
                    aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f, 
                    aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b)
VALUES (6, '20987654321', 'AUDIO Y VIDEO SAC', 'Compra de multimedia', 1, 18500.00, @FechaBase,
        1, GETDATE(), 1, GETDATE(), '192.168.1.100', 'PC-ADMIN', 0);

DECLARE @CompraId4 INT = SCOPE_IDENTITY();

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId4, pk_prod_id, 15, precio_salida, precio_entrada, (15 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Webcam Logitech C920';

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId4, pk_prod_id, 8, precio_salida, precio_entrada, (8 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Audífonos Sony WH-1000XM4';

-- Compra 5: Hace 15 días
SET @FechaBase = DATEADD(DAY, -15, GETDATE());

INSERT INTO compra (tipo_documento_id, ruc, razon_social, observacion, tipo_compra_id, total, fecha_registro, 
                    aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f, 
                    aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b)
VALUES (6, '20567890123', 'DISTRIBUIDORA TECH SAC', 'Reposición de stock', 1, 13250.00, @FechaBase,
        1, GETDATE(), 1, GETDATE(), '192.168.1.100', 'PC-ADMIN', 0);

DECLARE @CompraId5 INT = SCOPE_IDENTITY();

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId5, pk_prod_id, 3, precio_salida, precio_entrada, (3 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Laptop HP Core i5';

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId5, pk_prod_id, 40, precio_salida, precio_entrada, (40 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Mouse Logitech M185';

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId5, pk_prod_id, 20, precio_salida, precio_entrada, (20 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Teclado Mecánico RGB';

-- Compra 6: Hace 7 días
SET @FechaBase = DATEADD(DAY, -7, GETDATE());

INSERT INTO compra (tipo_documento_id, ruc, razon_social, observacion, tipo_compra_id, total, fecha_registro, 
                    aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f, 
                    aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b)
VALUES (6, '20876543210', 'IMPORTACIONES MONITOR PERU', 'Compra urgente', 1, 8500.00, @FechaBase,
        1, GETDATE(), 1, GETDATE(), '192.168.1.100', 'PC-ADMIN', 0);

DECLARE @CompraId6 INT = SCOPE_IDENTITY();

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId6, pk_prod_id, 5, precio_salida, precio_entrada, (5 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Monitor LG 24"';

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId6, pk_prod_id, 15, precio_salida, precio_entrada, (15 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Disco Duro Externo 1TB';

-- Compra 7: Hace 3 días
SET @FechaBase = DATEADD(DAY, -3, GETDATE());

INSERT INTO compra (tipo_documento_id, ruc, razon_social, observacion, tipo_compra_id, total, fecha_registro, 
                    aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f, 
                    aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b)
VALUES (6, '20123456789', 'TECH SUPPLIERS EIRL', 'Compra de componentes', 1, 9800.00, @FechaBase,
        1, GETDATE(), 1, GETDATE(), '192.168.1.100', 'PC-ADMIN', 0);

DECLARE @CompraId7 INT = SCOPE_IDENTITY();

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId7, pk_prod_id, 15, precio_salida, precio_entrada, (15 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Memoria RAM DDR4 8GB';

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId7, pk_prod_id, 10, precio_salida, precio_entrada, (10 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'SSD Kingston 480GB';

-- Compra 8: Hoy
INSERT INTO compra (tipo_documento_id, ruc, razon_social, observacion, tipo_compra_id, total, fecha_registro, 
                    aud_id_usu_cre_n, aud_fec_cre_f, aud_id_usu_mod_n, aud_fec_mod_f, 
                    aud_pc_ip_c, aud_pc_host_c, aud_es_eli_b)
VALUES (6, '20987654321', 'AUDIO Y VIDEO SAC', 'Compra del día', 1, 11200.00, GETDATE(),
        1, GETDATE(), 1, GETDATE(), '192.168.1.100', 'PC-ADMIN', 0);

DECLARE @CompraId8 INT = SCOPE_IDENTITY();

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId8, pk_prod_id, 10, precio_salida, precio_entrada, (10 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Webcam Logitech C920';

INSERT INTO detalle_compra (compra_id, producto_id, cantidad, precio_venta, precio_unitario, total)
SELECT @CompraId8, pk_prod_id, 4, precio_salida, precio_entrada, (4 * precio_entrada)
FROM producto_mae WHERE nombre_producto = 'Audífonos Sony WH-1000XM4';

GO

-- =============================================
-- 3. VERIFICAR DATOS INSERTADOS
-- =============================================
SELECT 
    'PRODUCTOS' as Tabla,
    COUNT(*) as Total
FROM producto_mae
WHERE aud_es_eli_b = 0

UNION ALL

SELECT 
    'COMPRAS' as Tabla,
    COUNT(*) as Total
FROM compra
WHERE aud_es_eli_b = 0

UNION ALL

SELECT 
    'DETALLES COMPRA' as Tabla,
    COUNT(*) as Total
FROM detalle_compra;

-- =============================================
-- 4. CONSULTA DE PRUEBA - MOVIMIENTOS DE STOCK
-- =============================================
SELECT 
    dc.detalle_venta_id as MovimientoId,
    p.pk_prod_id as ProductoId,
    p.nombre_producto as NombreProducto,
    'Compra' as TipoMovimiento,
    CAST(dc.cantidad as INT) as Cantidad,
    dc.precio_unitario as PrecioUnitario,
    dc.total as Total,
    c.fecha_registro as FechaMovimiento,
    p.stock as StockResultante
FROM detalle_compra dc
LEFT JOIN compra c ON dc.compra_id = c.pk_com_id
LEFT JOIN producto_mae p ON dc.producto_id = p.pk_prod_id
WHERE c.aud_es_eli_b = 0
ORDER BY c.fecha_registro DESC;

-- =============================================
-- 5. RESUMEN DE DATOS INSERTADOS
-- =============================================
DECLARE @TotalProductos INT;
DECLARE @TotalCompras INT;
DECLARE @TotalDetalles INT;

SELECT @TotalProductos = COUNT(*) FROM producto_mae WHERE aud_es_eli_b = 0;
SELECT @TotalCompras = COUNT(*) FROM compra WHERE aud_es_eli_b = 0;
SELECT @TotalDetalles = COUNT(*) FROM detalle_compra;

PRINT '✓ Datos de movimientos de stock insertados correctamente';
PRINT '✓ Total de productos: ' + CAST(@TotalProductos AS VARCHAR);
PRINT '✓ Total de compras: ' + CAST(@TotalCompras AS VARCHAR);
PRINT '✓ Total de detalles: ' + CAST(@TotalDetalles AS VARCHAR);
