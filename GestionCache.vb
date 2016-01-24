Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Web.Caching
Imports System.Web.Script.Serialization 'incluir la referencia System.Web.Extensions;

Namespace Negocio

    Public Class GestionCache
	
        'Utilizada solo por las sobrecargas
        'el parámetro bSerializar indica si en la caché guardamos objetos o cadenas de json que serializar y deserializar internamente
        Private Shared Function GetSetCache(Of T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) _
        (
            fPunteroFuncionGet0 As Func(Of T),
            fPunteroFuncionGet1 As Func(Of T1, T),
            fPunteroFuncionGet2 As Func(Of T1, T2, T),
            fPunteroFuncionGet3 As Func(Of T1, T2, T3, T),
            fPunteroFuncionGet4 As Func(Of T1, T2, T3, T4, T),
            fPunteroFuncionGet5 As Func(Of T1, T2, T3, T4, T5, T),
            fPunteroFuncionGet6 As Func(Of T1, T2, T3, T4, T5, T6, T),
            fPunteroFuncionGet7 As Func(Of T1, T2, T3, T4, T5, T6, T7, T),
            fPunteroFuncionGet8 As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T),
            fPunteroFuncionGet9 As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T),
            fPunteroFuncionGet10 As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T),
            sClaveCache As String,
            iMinutosTTL As Integer,
            bSerializar As Boolean,
            oParametro1 As T1,
            oParametro2 As T2,
            oParametro3 As T3,
            oParametro4 As T4,
            oParametro5 As T5,
            oParametro6 As T6,
            oParametro7 As T7,
            oParametro8 As T8,
            oParametro9 As T9,
            oParametro10 As T10
        ) As T
            Dim oRetorno As T = Nothing
            'Por si el tipo T no admite nulos
            Dim sResultado As String = String.Empty
            Dim oCache = HttpContext.Current.Cache
            Dim CacheStampede As CacheStampede

            If bSerializar Then
                sResultado = DirectCast(oCache.[Get](sClaveCache), String)
                'si no hay nada en cache, prevenimos una "Cache Stampede"
                If String.IsNullOrEmpty(sResultado) Then
                    'si no hay nada en cache, prevenimos una "Cache Stampede"
                    'TODO
                End If
            Else
                oRetorno = DirectCast(oCache.[Get](sClaveCache), T)
                'si no hay nada en cache, prevenimos una "Cache Stampede"
                If oRetorno Is Nothing Then
                    'si no hay nada en cache, prevenimos una "Cache Stampede"
                    'TODO
                End If
            End If

            If (Not bSerializar AndAlso oRetorno Is Nothing) OrElse (bSerializar AndAlso String.IsNullOrEmpty(sResultado)) Then
                'No hay nada en la caché, obtenemos:

                'Llamadas según parámetros.ini
                If fPunteroFuncionGet0 IsNot Nothing Then
                    oRetorno = fPunteroFuncionGet0()
                ElseIf fPunteroFuncionGet1 IsNot Nothing Then
                    oRetorno = fPunteroFuncionGet1(oParametro1)
                ElseIf fPunteroFuncionGet2 IsNot Nothing Then
                    oRetorno = fPunteroFuncionGet2(oParametro1, oParametro2)
                ElseIf fPunteroFuncionGet3 IsNot Nothing Then
                    oRetorno = fPunteroFuncionGet3(oParametro1, oParametro2, oParametro3)
                ElseIf fPunteroFuncionGet4 IsNot Nothing Then
                    oRetorno = fPunteroFuncionGet4(oParametro1, oParametro2, oParametro3, oParametro4)
                ElseIf fPunteroFuncionGet5 IsNot Nothing Then
                    oRetorno = fPunteroFuncionGet5(oParametro1, oParametro2, oParametro3, oParametro4, oParametro5)
                ElseIf fPunteroFuncionGet6 IsNot Nothing Then
                    oRetorno = fPunteroFuncionGet6(oParametro1, oParametro2, oParametro3, oParametro4, oParametro5, oParametro6)
                ElseIf fPunteroFuncionGet7 IsNot Nothing Then
                    oRetorno = fPunteroFuncionGet7(oParametro1, oParametro2, oParametro3, oParametro4, oParametro5, oParametro6, oParametro7)
                ElseIf fPunteroFuncionGet8 IsNot Nothing Then
                    oRetorno = fPunteroFuncionGet8(oParametro1, oParametro2, oParametro3, oParametro4, oParametro5, oParametro6, oParametro7, oParametro8)
                ElseIf fPunteroFuncionGet9 IsNot Nothing Then
                    oRetorno = fPunteroFuncionGet9(oParametro1, oParametro2, oParametro3, oParametro4, oParametro5, oParametro6, oParametro7, oParametro8, oParametro9)
                ElseIf fPunteroFuncionGet10 IsNot Nothing Then
                    oRetorno = fPunteroFuncionGet10(oParametro1, oParametro2, oParametro3, oParametro4, oParametro5, oParametro6, oParametro7, oParametro8, oParametro9, oParametro10)
                End If
                'Llamadas según parámetros.fin

                'Guardamos en Caché si no es nulo
                If oRetorno IsNot Nothing Then
                    If bSerializar Then
                        sResultado = Serializar(Of T)(oRetorno)
                        If Not String.IsNullOrEmpty(sResultado) Then
                            'guardamos en cache para las siguientes peticiones
                            oCache.Add(sClaveCache, sResultado, Nothing, DateTime.Now.AddMinutes(iMinutosTTL), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.[Default], Nothing)
                        End If
                    Else
                        oCache.Add(sClaveCache, oRetorno, Nothing, DateTime.Now.AddMinutes(iMinutosTTL), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.[Default], Nothing)
                    End If
                End If
            End If

            If bSerializar AndAlso oRetorno Is Nothing AndAlso Not String.IsNullOrEmpty(sResultado) Then
                oRetorno = Deserializar(Of T)(sResultado)
            End If

            Return oRetorno
        End Function

        '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ SobreCargas.ini @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@//
        Public Shared Function GetSetCache(Of T) _
        (
            fPunteroFuncionGet As Func(Of T),
            sClaveCache As String,
            iMinutosTTL As Integer,
            bSerializar As Boolean
        ) As T
            Return GetSetCache(Of T, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object) _
            (
                fPunteroFuncionGet,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing
            )
        End Function
        Public Shared Function GetSetCache(Of T, T1) _
        (
            fPunteroFuncionGet As Func(Of T1, T),
            sClaveCache As String,
            iMinutosTTL As Integer,
            bSerializar As Boolean,
            oParametro1 As T1
        ) As T
            Return GetSetCache(Of T, T1, Object, Object, Object, Object, Object, Object, Object, Object, Object) _
                (
                    Nothing,
                    fPunteroFuncionGet,
                    Nothing,
                    Nothing,
                    Nothing,
                    Nothing,
                    Nothing,
                    Nothing,
                    Nothing,
                    Nothing,
                    Nothing,
                    sClaveCache,
                    iMinutosTTL,
                    bSerializar,
                    oParametro1,
                    Nothing,
                    Nothing,
                    Nothing,
                    Nothing,
                    Nothing,
                    Nothing,
                    Nothing,
                    Nothing,
                    Nothing
                )
        End Function
        Public Shared Function GetSetCache(Of T, T1, T2) _
        (
            fPunteroFuncionGet As Func(Of T1, T2, T),
            sClaveCache As String,
            iMinutosTTL As Integer,
            bSerializar As Boolean,
            oParametro1 As T1,
            oParametro2 As T2
        ) As T
            Return GetSetCache(Of T, T1, T2, Object, Object, Object, Object, Object, Object, Object, Object) _
            (
                Nothing,
                Nothing,
                fPunteroFuncionGet,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing
            )
        End Function
        Public Shared Function GetSetCache(Of T, T1, T2, T3) _
        (
            fPunteroFuncionGet As Func(Of T1, T2, T3, T),
            sClaveCache As String,
            iMinutosTTL As Integer,
            bSerializar As Boolean,
            oParametro1 As T1,
            oParametro2 As T2,
            oParametro3 As T3
        ) As T
            Return GetSetCache(Of T, T1, T2, T3, Object, Object, Object, Object, Object, Object, Object) _
            (
                Nothing,
                Nothing,
                Nothing,
                fPunteroFuncionGet,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                oParametro3,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing
            )
        End Function
        Public Shared Function GetSetCache(Of T, T1, T2, T3, T4) _
        (
            fPunteroFuncionGet As Func(Of T1, T2, T3, T4, T),
            sClaveCache As String,
            iMinutosTTL As Integer,
            bSerializar As Boolean,
            oParametro1 As T1,
            oParametro2 As T2,
            oParametro3 As T3,
            oParametro4 As T4
        ) As T
            Return GetSetCache(Of T, T1, T2, T3, T4, Object, Object, Object, Object, Object, Object) _
            (
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                fPunteroFuncionGet,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                oParametro3,
                oParametro4,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing
            )
        End Function
        Public Shared Function GetSetCache(Of T, T1, T2, T3, T4, T5) _
        (
            fPunteroFuncionGet As Func(Of T1, T2, T3, T4, T5, T),
            sClaveCache As String,
            iMinutosTTL As Integer,
            bSerializar As Boolean,
            oParametro1 As T1,
            oParametro2 As T2,
            oParametro3 As T3,
            oParametro4 As T4,
            oParametro5 As T5
        ) As T
            Return GetSetCache(Of T, T1, T2, T3, T4, T5, Object, Object, Object, Object, Object) _
            (
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                fPunteroFuncionGet,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                oParametro3,
                oParametro4,
                oParametro5,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing
            )
        End Function
        Public Shared Function GetSetCache(Of T, T1, T2, T3, T4, T5, T6) _
        (
            fPunteroFuncionGet As Func(Of T1, T2, T3, T4, T5, T6, T),
            sClaveCache As String,
            iMinutosTTL As Integer,
            bSerializar As Boolean,
            oParametro1 As T1,
            oParametro2 As T2,
            oParametro3 As T3,
            oParametro4 As T4,
            oParametro5 As T5,
            oParametro6 As T6
        ) As T
            Return GetSetCache(Of T, T1, T2, T3, T4, T5, T6, Object, Object, Object, Object) _
            (
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                fPunteroFuncionGet,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                sClaveCache,
                iMinutosTTL,
                bSerializar,
                oParametro1,
                oParametro2,
                oParametro3,
                oParametro4,
                oParametro5,
                oParametro6,
                Nothing,
                Nothing,
                Nothing,
                Nothing
            )
        End Function
        Public Shared Function GetSetCache(Of T, T1, T2, T3, T4, T5, T6, T7) _
        (
            fPunteroFuncionGet As Func(Of T1, T2, T3, T4, T5, T6, T7, T),
            sClaveCache As String,
            iMinutosTTL As Integer,
            bSerializar As Boolean,
            oParametro1 As T1,
            oParametro2 As T2,
            oParametro3 As T3,
            oParametro4 As T4,
            oParametro5 As T5,
            oParametro6 As T6,
            oParametro7 As T7
        ) As T
            Return GetSetCache(Of T, T1, T2, T3, T4, T5, T6, T7, Object, Object, Object) _
            (
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                fPunteroFuncionGet,
                Nothing,
                Nothing,
                Nothing,
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
                Nothing,
                Nothing,
                Nothing
            )
        End Function
        Public Shared Function GetSetCache(Of T, T1, T2, T3, T4, T5, T6, T7, T8) _
        (
            fPunteroFuncionGet As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T),
            sClaveCache As String,
            iMinutosTTL As Integer,
            bSerializar As Boolean,
            oParametro1 As T1,
            oParametro2 As T2,
            oParametro3 As T3,
            oParametro4 As T4,
            oParametro5 As T5,
            oParametro6 As T6,
            oParametro7 As T7,
            oParametro8 As T8
        ) As T
            Return GetSetCache(Of T, T1, T2, T3, T4, T5, T6, T7, T8, Object, Object) _
            (
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                fPunteroFuncionGet,
                Nothing,
                Nothing,
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
                Nothing,
                Nothing
            )
        End Function
        Public Shared Function GetSetCache(Of T, T1, T2, T3, T4, T5, T6, T7, T8, T9) _
        (
            fPunteroFuncionGet As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T),
            sClaveCache As String,
            iMinutosTTL As Integer,
            bSerializar As Boolean,
            oParametro1 As T1,
            oParametro2 As T2,
            oParametro3 As T3,
            oParametro4 As T4,
            oParametro5 As T5,
            oParametro6 As T6,
            oParametro7 As T7,
            oParametro8 As T8,
            oParametro9 As T9
        ) As T
            Return GetSetCache(Of T, T1, T2, T3, T4, T5, T6, T7, T8, T9, Object) _
            (
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                fPunteroFuncionGet,
                Nothing,
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
                Nothing)
        End Function
        Public Shared Function GetSetCache(Of T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) _
        (
            fPunteroFuncionGet As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T),
            sClaveCache As String,
            iMinutosTTL As Integer,
            bSerializar As Boolean,
            oParametro1 As T1,
            oParametro2 As T2,
            oParametro3 As T3,
            oParametro4 As T4,
            oParametro5 As T5,
            oParametro6 As T6,
            oParametro7 As T7,
            oParametro8 As T8,
            oParametro9 As T9,
            oParametro10 As T10
        ) As T
            Return GetSetCache(Of T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) _
            (
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
                Nothing,
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
            )
        End Function
        '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ SobreCargas.fin @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@//

        'Importante: Para serializar necesitamos propiedades de Get y Set del objeto tipo T
        Public Shared Function Serializar(Of T)(oObjeto As T) As String
            Dim sr As New JavaScriptSerializer()
            Return sr.Serialize(oObjeto)
        End Function
        Public Shared Function Deserializar(Of T)(sJson As String) As T
            Dim sr As New JavaScriptSerializer()
            Return sr.Deserialize(Of T)(sJson)
        End Function
    End Class
End Namespace