Public Class DLDistance
	Public Function DamerauLevenshteinDistance(ByVal str1 As String, ByVal str2 As String) As Integer

		Dim d As Integer(,)

		ReDim d(str1.Length + 1, str2.Length + 1)

		For i As Integer = 0 To str1.Length
			d(i, 0) = i
		Next

		For j As Integer = 0 To str2.Length
			d(0, j) = j
		Next

		Dim cost As Integer

		For i As Integer = 1 To str1.Length

			For j As Integer = 1 To str2.Length

				If str1(i - 1) = str2(j - 1) Then
					cost = 0
				Else
					cost = 1
				End If

				d(i, j) = Math.Min(d(i - 1, j) + 1, Math.Min(d(i, j - 1) + 1, d(i - 1, j - 1) + cost))

				If (i > 2 And j > 2) AndAlso (str1(i - 1) = str2(j - 2) And str1(i - 2) = str2(j - 1)) Then

					d(i, j) = Math.Min(d(i, j), d(i - 2, j - 2) + cost)

				End If

			Next

		Next

		Return d(str1.Length, str2.Length)

	End Function
End Class
