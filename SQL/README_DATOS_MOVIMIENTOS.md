# 📊 Datos de Prueba - Movimientos de Stock

## 📝 Descripción

Este script genera datos de prueba para el reporte de **Movimientos de Stock** del módulo `cl-reportes`. Inserta:

- ✅ **10 productos** de tecnología (laptops, periféricos, componentes)
- ✅ **8 compras** distribuidas en los últimos 3 meses
- ✅ **30+ detalles de compra** con diferentes cantidades y precios
- ✅ Movimientos con fechas escalonadas para simular un historial real

## 🎯 Columnas que se visualizan en el Frontend

```html
<th>Producto</th>          <!-- nombre_producto -->
<th>Tipo</th>              <!-- 'Compra' -->
<th>Cantidad</th>          <!-- cantidad -->
<th>Precio Unit.</th>      <!-- precio_unitario -->
<th>Total</th>             <!-- total -->
<th>Fecha</th>             <!-- fecha_registro -->
```

## 🚀 Cómo ejecutar

### Opción 1: SQL Server Management Studio (SSMS)
```sql
1. Abre SSMS y conéctate a tu servidor
2. Abre el archivo: INSERT_DATOS_MOVIMIENTOS_STOCK.sql
3. Asegúrate que la base de datos sea BD_INKACAO
4. Presiona F5 o clic en "Execute"
```

### Opción 2: Azure Data Studio
```sql
1. Conecta a tu servidor Azure SQL
2. Carga el archivo SQL
3. Ejecuta todo el script
```

### Opción 3: Terminal (sqlcmd)
```bash
sqlcmd -S <servidor> -d BD_INKACAO -U <usuario> -P <password> -i INSERT_DATOS_MOVIMIENTOS_STOCK.sql
```

## 📋 Datos que genera

### Productos (10 items)
| Producto | Precio Entrada | Precio Salida | Stock Inicial |
|----------|----------------|---------------|---------------|
| Laptop HP Core i5 | S/2,500 | S/3,200 | 50 |
| Mouse Logitech M185 | S/35 | S/55 | 150 |
| Teclado Mecánico RGB | S/180 | S/280 | 80 |
| Monitor LG 24" | S/550 | S/750 | 30 |
| Impresora HP LaserJet | S/800 | S/1,100 | 25 |
| Disco Duro Externo 1TB | S/180 | S/250 | 60 |
| Memoria RAM DDR4 8GB | S/120 | S/180 | 100 |
| SSD Kingston 480GB | S/200 | S/290 | 45 |
| Webcam Logitech C920 | S/280 | S/380 | 40 |
| Audífonos Sony WH-1000XM4 | S/950 | S/1,300 | 20 |

### Compras (8 registros)
| Fecha | Proveedor | Total | # Items |
|-------|-----------|-------|---------|
| Hace 3 meses | DISTRIBUIDORA TECH SAC | S/15,500 | 3 |
| Hace 2.5 meses | IMPORTACIONES MONITOR PERU | S/22,000 | 3 |
| Hace 2 meses | TECH SUPPLIERS EIRL | S/12,000 | 2 |
| Hace 1 mes | AUDIO Y VIDEO SAC | S/18,500 | 2 |
| Hace 15 días | DISTRIBUIDORA TECH SAC | S/13,250 | 3 |
| Hace 7 días | IMPORTACIONES MONITOR PERU | S/8,500 | 2 |
| Hace 3 días | TECH SUPPLIERS EIRL | S/9,800 | 2 |
| Hoy | AUDIO Y VIDEO SAC | S/11,200 | 2 |

**Total movimientos generados: 30+ detalles de compra**

## 🔍 Consultas de verificación

### Ver todos los movimientos
```sql
SELECT 
    p.nombre_producto as Producto,
    'Compra' as Tipo,
    CAST(dc.cantidad as INT) as Cantidad,
    dc.precio_unitario as PrecioUnitario,
    dc.total as Total,
    c.fecha_registro as Fecha
FROM detalle_compra dc
INNER JOIN compra c ON dc.compra_id = c.pk_com_id
INNER JOIN producto_mae p ON dc.producto_id = p.pk_prod_id
WHERE c.aud_es_eli_b = 0
ORDER BY c.fecha_registro DESC;
```

### Ver movimientos del último mes
```sql
SELECT 
    p.nombre_producto,
    COUNT(*) as TotalMovimientos,
    SUM(dc.cantidad) as CantidadTotal,
    SUM(dc.total) as MontoTotal
FROM detalle_compra dc
INNER JOIN compra c ON dc.compra_id = c.pk_com_id
INNER JOIN producto_mae p ON dc.producto_id = p.pk_prod_id
WHERE c.fecha_registro >= DATEADD(MONTH, -1, GETDATE())
AND c.aud_es_eli_b = 0
GROUP BY p.nombre_producto
ORDER BY MontoTotal DESC;
```

### Ver stock actual
```sql
SELECT 
    nombre_producto,
    stock as StockActual,
    precio_entrada,
    precio_salida,
    (stock * precio_entrada) as ValorInventario
FROM producto_mae
WHERE aud_es_eli_b = 0
ORDER BY ValorInventario DESC;
```

## 🧪 Probar en Postman

Una vez ejecutado el script, prueba el endpoint:

```http
GET http://localhost:18548/api/reportes/movimientos-stock
```

### Con filtros (opcional)
```http
GET http://localhost:18548/api/reportes/movimientos-stock?fechaInicio=2025-10-01&fechaFin=2025-11-10
```

## 📊 Resultado esperado en el Frontend

```json
{
  "movimientos": [
    {
      "movimientoId": 1,
      "productoId": 1,
      "nombreProducto": "Laptop HP Core i5",
      "tipoMovimiento": "Compra",
      "cantidad": 5,
      "precioUnitario": 2500.00,
      "total": 12500.00,
      "fechaMovimiento": "2025-08-10T10:30:00",
      "stockResultante": 50
    },
    // ... más movimientos
  ]
}
```

## ⚠️ Notas Importantes

1. **El script NO elimina datos existentes** - Solo inserta nuevos registros
2. **Verifica antes** que no existan productos con los mismos nombres
3. **Si ya ejecutaste el script**, no lo vuelvas a ejecutar o tendrás duplicados
4. **Para limpiar y empezar de nuevo**:
   ```sql
   DELETE FROM detalle_compra;
   DELETE FROM compra WHERE ruc IN ('20567890123', '20876543210', '20123456789', '20987654321');
   DELETE FROM producto_mae WHERE nombre_producto LIKE 'Laptop HP%' OR nombre_producto LIKE 'Mouse%';
   ```

## 🎨 Visualización en Angular

El componente mostrará estos datos en una tabla Bootstrap con:
- ✅ Badge verde para "Compra"
- ✅ Badge rojo para "Venta" (si existieran)
- ✅ Formato de moneda para precios
- ✅ Formato de fecha corta
- ✅ Filtros por rango de fechas

## 📞 Soporte

Si tienes problemas ejecutando el script:

1. Verifica que estés conectado a la BD correcta: `BD_INKACAO`
2. Asegúrate que tu usuario tenga permisos de INSERT
3. Revisa que las tablas `producto_mae`, `compra` y `detalle_compra` existan
4. Consulta los mensajes de error en la consola SQL

---

**✨ Script creado para demostración del módulo de Reportes**
**🔧 Proyecto Capstone - Sistema de Gestión de Almacenes**
