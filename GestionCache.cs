using System;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Serialization; //incluir la referencia System.Web.Extensions;

namespace Negocio
{
    public class GestionCache
    {
        //Utilizada solo por las sobrecargas
        //el parámetro bSerializar indica si en la caché guardamos objetos o cadenas de json que serializar y deserializar internamente
        private static T GetSetCache<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        (
            Func<T> fPunteroFuncionGet0,
            Func<T1, T> fPunteroFuncionGet1,
            Func<T1, T2, T> fPunteroFuncionGet2,
            Func<T1, T2, T3, T> fPunteroFuncionGet3,
            Func<T1, T2, T3, T4, T> fPunteroFuncionGet4,
            Func<T1, T2, T3, T4, T5, T> fPunteroFuncionGet5,
            Func<T1, T2, T3, T4, T5, T6, T> fPunteroFuncionGet6,
            Func<T1, T2, T3, T4, T5, T6, T7, T> fPunteroFuncionGet7,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T> fPunteroFuncionGet8,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T> fPunteroFuncionGet9,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T> fPunteroFuncionGet10,
            string sClaveCache,
            int iMinutosTTL,
            bool bSerializar,
            T1 oParametro1,
            T2 oParametro2,
            T3 oParametro3,
            T4 oParametro4,
            T5 oParametro5,
            T6 oParametro6,
            T7 oParametro7,
            T8 oParametro8,
            T9 oParametro9,
            T10 oParametro10
        )
        {
            T oRetorno = default(T); //Por si el tipo T no admite nulos
            string sResultado = string.Empty;
            var oCache = HttpContext.Current.Cache;

            if (bSerializar)
            {
                sResultado = (string)oCache.Get(sClaveCache);
                //si no hay nada en cache, prevenimos una "Cache Stampede"
                if (string.IsNullOrEmpty(sResultado))
                {
                    //si no hay nada en cache, prevenimos una "Cache Stampede"
                    //TODO
                }
            }
            else {
                oRetorno = (T)oCache.Get(sClaveCache);
                if (oRetorno == null)
                {
					//si no hay nada en cache, prevenimos una "Cache Stampede"
                    //TODO
                }
            }

            if ((!bSerializar && oRetorno == null) || (bSerializar && string.IsNullOrEmpty(sResultado)))
            {
                //No hay nada en la caché, obtenemos:

                //Llamadas según parámetros.ini
                if (fPunteroFuncionGet0 != null)
                {
                    oRetorno = fPunteroFuncionGet0();
                }
                else if (fPunteroFuncionGet1 != null)
                {
                    oRetorno = fPunteroFuncionGet1(oParametro1);
                }
                else if (fPunteroFuncionGet2 != null)
                {
                    oRetorno = fPunteroFuncionGet2(oParametro1, oParametro2);
                }
                else if (fPunteroFuncionGet3 != null)
                {
                    oRetorno = fPunteroFuncionGet3(oParametro1, oParametro2, oParametro3);
                }
                else if (fPunteroFuncionGet4 != null)
                {
                    oRetorno = fPunteroFuncionGet4(oParametro1, oParametro2, oParametro3, oParametro4);
                }
                else if (fPunteroFuncionGet5 != null)
                {
                    oRetorno = fPunteroFuncionGet5(oParametro1, oParametro2, oParametro3, oParametro4, oParametro5);
                }
                else if (fPunteroFuncionGet6 != null)
                {
                    oRetorno = fPunteroFuncionGet6(oParametro1, oParametro2, oParametro3, oParametro4, oParametro5, oParametro6);
                }
                else if (fPunteroFuncionGet7 != null)
                {
                    oRetorno = fPunteroFuncionGet7(oParametro1, oParametro2, oParametro3, oParametro4, oParametro5, oParametro6, oParametro7);
                }
                else if (fPunteroFuncionGet8 != null)
                {
                    oRetorno = fPunteroFuncionGet8(oParametro1, oParametro2, oParametro3, oParametro4, oParametro5, oParametro6, oParametro7, oParametro8);
                }
                else if (fPunteroFuncionGet9 != null)
                {
                    oRetorno = fPunteroFuncionGet9(oParametro1, oParametro2, oParametro3, oParametro4, oParametro5, oParametro6, oParametro7, oParametro8, oParametro9);
                }
                else if (fPunteroFuncionGet10 != null)
                {
                    oRetorno = fPunteroFuncionGet10(oParametro1, oParametro2, oParametro3, oParametro4, oParametro5, oParametro6, oParametro7, oParametro8, oParametro9, oParametro10);
                }
                //Llamadas según parámetros.fin

                //Guardamos en Caché si no es nulo
                if (oRetorno != null)
                {
                    if (bSerializar)
                    {
                        sResultado = Serializar<T>(oRetorno);
                        if (!string.IsNullOrEmpty(sResultado))
                        {
                            //guardamos en cache para las siguientes peticiones
                            oCache.Add(sClaveCache, sResultado, null, DateTime.Now.AddMinutes(iMinutosTTL), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                        }
                    }
                    else {
                        oCache.Add(sClaveCache, oRetorno, null, DateTime.Now.AddMinutes(iMinutosTTL), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                    }
                }
            }

            if (bSerializar && oRetorno == null && !string.IsNullOrEmpty(sResultado))
            {
                oRetorno = Deserializar<T>(sResultado);
            }

            return oRetorno;
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ SobreCargas.ini @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@//
        public static T GetSetCache<T>
        (
            Func<T> fPunteroFuncionGet,
            string sClaveCache,
            int iMinutosTTL,
            bool bSerializar
        )
        {
            return GetSetCache<T, object, object, object, object, object, object, object, object, object, object>
            (
                fPunteroFuncionGet,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );
        }
        public static T GetSetCache<T, T1>
        (
            Func<T1, T> fPunteroFuncionGet,
            string sClaveCache,
            int iMinutosTTL,
            bool bSerializar,
            T1 oParametro1
        )
        {
            return GetSetCache<T, T1, object, object, object, object, object, object, object, object, object>
            (
                null,
                fPunteroFuncionGet,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );
        }
        public static T GetSetCache<T, T1, T2>
        (
            Func<T1, T2, T> fPunteroFuncionGet,
            string sClaveCache,
            int iMinutosTTL,
            bool bSerializar,
            T1 oParametro1,
            T2 oParametro2
        )
        {
            return GetSetCache<T, T1, T2, object, object, object, object, object, object, object, object>
            (
                null,
                null,
                fPunteroFuncionGet,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );
        }
        public static T GetSetCache<T, T1, T2, T3>
        (
            Func<T1, T2, T3, T> fPunteroFuncionGet,
            string sClaveCache,
            int iMinutosTTL,
            bool bSerializar,
            T1 oParametro1,
            T2 oParametro2,
            T3 oParametro3
        )
        {
            return GetSetCache<T, T1, T2, T3, object, object, object, object, object, object, object>
            (
                null,
                null,
                null,
                fPunteroFuncionGet,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                oParametro3,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );
        }
        public static T GetSetCache<T, T1, T2, T3, T4>
        (
            Func<T1, T2, T3, T4, T> fPunteroFuncionGet,
            string sClaveCache,
            int iMinutosTTL,
            bool bSerializar,
            T1 oParametro1,
            T2 oParametro2,
            T3 oParametro3,
            T4 oParametro4
        )
        {
            return GetSetCache<T, T1, T2, T3, T4, object, object, object, object, object, object>
            (
                null,
                null,
                null,
                null,
                fPunteroFuncionGet,
                null,
                null,
                null,
                null,
                null,
                null,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                oParametro3,
                oParametro4,
                null,
                null,
                null,
                null,
                null,
                null
            );
        }
        public static T GetSetCache<T, T1, T2, T3, T4, T5>
        (
            Func<T1, T2, T3, T4, T5, T> fPunteroFuncionGet,
            string sClaveCache,
            int iMinutosTTL,
            bool bSerializar,
            T1 oParametro1,
            T2 oParametro2,
            T3 oParametro3,
            T4 oParametro4,
            T5 oParametro5
        )
        {
            return GetSetCache<T, T1, T2, T3, T4, T5, object, object, object, object, object>
            (
                null,
                null,
                null,
                null,
                null,
                fPunteroFuncionGet,
                null,
                null,
                null,
                null,
                null,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                oParametro3,
                oParametro4,
                oParametro5,
                null,
                null,
                null,
                null,
                null
            );
        }
        public static T GetSetCache<T, T1, T2, T3, T4, T5, T6>
        (
            Func<T1, T2, T3, T4, T5, T6, T> fPunteroFuncionGet,
            string sClaveCache,
            int iMinutosTTL,
            bool bSerializar,
            T1 oParametro1,
            T2 oParametro2,
            T3 oParametro3,
            T4 oParametro4,
            T5 oParametro5,
            T6 oParametro6
        )
        {
            return GetSetCache<T, T1, T2, T3, T4, T5, T6, object, object, object, object>
            (
                null,
                null,
                null,
                null,
                null,
                null,
                fPunteroFuncionGet,
                null,
                null,
                null,
                null,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                oParametro3,
                oParametro4,
                oParametro5,
                oParametro6,
                null,
                null,
                null,
                null
            );
        }
        public static T GetSetCache<T, T1, T2, T3, T4, T5, T6, T7>
        (
            Func<T1, T2, T3, T4, T5, T6, T7, T> fPunteroFuncionGet,
            string sClaveCache,
            int iMinutosTTL,
            bool bSerializar,
            T1 oParametro1,
            T2 oParametro2,
            T3 oParametro3,
            T4 oParametro4,
            T5 oParametro5,
            T6 oParametro6,
            T7 oParametro7
        )
        {
            return GetSetCache<T, T1, T2, T3, T4, T5, T6, T7, object, object, object>
            (
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                fPunteroFuncionGet,
                null,
                null,
                null,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                oParametro3,
                oParametro4,
                oParametro5,
                oParametro6,
                oParametro7,
                null,
                null,
                null
            );
        }
        public static T GetSetCache<T, T1, T2, T3, T4, T5, T6, T7, T8>
        (
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T> fPunteroFuncionGet,
            string sClaveCache,
            int iMinutosTTL,
            bool bSerializar,
            T1 oParametro1,
            T2 oParametro2,
            T3 oParametro3,
            T4 oParametro4,
            T5 oParametro5,
            T6 oParametro6,
            T7 oParametro7,
            T8 oParametro8
        )
        {
            return GetSetCache<T, T1, T2, T3, T4, T5, T6, T7, T8, object, object>
            (
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                fPunteroFuncionGet,
                null,
                null,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                oParametro3,
                oParametro4,
                oParametro5,
                oParametro6,
                oParametro7,
                oParametro8,
                null,
                null
            );
        }
        public static T GetSetCache<T, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        (
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T> fPunteroFuncionGet,
            string sClaveCache,
            int iMinutosTTL,
            bool bSerializar,
            T1 oParametro1,
            T2 oParametro2,
            T3 oParametro3,
            T4 oParametro4,
            T5 oParametro5,
            T6 oParametro6,
            T7 oParametro7,
            T8 oParametro8,
            T9 oParametro9
        )
        {
            return GetSetCache<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, object>
            (
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                fPunteroFuncionGet,
                null,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                oParametro3,
                oParametro4,
                oParametro5,
                oParametro6,
                oParametro7,
                oParametro8,
                oParametro9,
                null
            );
        }
        public static T GetSetCache<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        (
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T> fPunteroFuncionGet,
            string sClaveCache,
            int iMinutosTTL,
            bool bSerializar,
            T1 oParametro1,
            T2 oParametro2,
            T3 oParametro3,
            T4 oParametro4,
            T5 oParametro5,
            T6 oParametro6,
            T7 oParametro7,
            T8 oParametro8,
            T9 oParametro9,
            T10 oParametro10
        )
        {
            return GetSetCache<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
            (
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                fPunteroFuncionGet,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                oParametro3,
                oParametro4,
                oParametro5,
                oParametro6,
                oParametro7,
                oParametro8,
                oParametro9,
                oParametro10
            );
        }
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ SobreCargas.fin @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@//


        //Importante: Para serializar necesitamos propiedades de Get y Set del objeto tipo T
        public static string Serializar<T>(T oObjeto)
        {
            JavaScriptSerializer sr = new JavaScriptSerializer();
            return sr.Serialize(oObjeto);
        }
        public static T Deserializar<T>(string sJson)
        {
            JavaScriptSerializer sr = new JavaScriptSerializer();
            return sr.Deserialize<T>(sJson);
        }
    }
}