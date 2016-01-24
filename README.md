# GestionCache
Encapsular Gestión de Caché para Simplificación de Código

Uso:
	1) Ejemplo sin parámetros en DescriptorFuncion
		Func<TipoRetorno> fDescriptorFuncion = NombreFuncion;
		TipoRetorno oRetorno = GestionCache.GetSetCache(fDescriptorFuncion, sClaveCache, iMinutosTTL, bNoCachearObjetosSinoSerializarEnJson)
	2) Ejemplos con 3 parámetros en DescriptorFuncion de distintos tipos
		Func<TipoParam1, TipoParam2, TipoParam3, TipoRetorno> fDescriptorFuncion = DescriptorFuncion;
		TipoRetorno oRetorno = GestionCache.GetSetCache(fDescriptorFuncion, sClaveCache, iMinutosTTL, bNoCachearObjetosSinoSerializarEnJson, sParam1, iParam2, aParam3)
	  
Límites:
	1) Tener en cuenta que cuando especificamos que se serialice, necesitamos haber definido propiedades de Get y Set para el TipoRetorno
	2) Se han definido sobrecargas para llamadas hasta 10 parámetros.