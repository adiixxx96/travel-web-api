El proyecto travel web es una API creada para una web de reservas de viajes.

Por necesidades de la web tenemos tres entidades:
- Usuarios: con los atributos id, nombre, usuario, contraseña, rol y fecha de creación.
- Viajes: con los atributos id, destino, fecha de salida, fecha de llegada, precio, puestos disponibles y booleano está lleno.
- Reservas: con los atributos id, id de usuario, id de viaje, fecha de la reserva y precio final.

La API está en lenguaje .NET y los datos se almacenan en una bbdd en la nube de Azure. Se siguió la metodología "Code first", haciendo una migración del modelo de datos a la bbdd.

Para esta web se necesita una API que realice las siguientes llamadas: 
- Usuario: registrar usuario, loguear usuario, obtener todos los usuarios, editar usuario y eliminar usuario.
- Viaje: añadir un viaje, listar todos los viajes, obtener un viaje, editar un viaje y eliminar viaje. Además, los viajes se pueden obtener filtrando por destino y con distintos parámetros para ordenar los resultados obtenidos.
- Reserva: hacer una reserva, listar todas las reservas, eliminar una reserva, obtener todos los viajes reservados por un usuario y obtener todos los usuarios que han reservado un viaje.

La API tiene un swagger para poder probar todas las acciones, además de que adjunto al proyecto la colección de postman con ejemplos de cada llamada.