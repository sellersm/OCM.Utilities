Public NotInheritable Class JaroWinklerDistance
	Private Sub New()
	End Sub
	' The Winkler modification will not be applied unless the 
	'     * percent match was at or above the mWeightThreshold percent 
	'     * without the modification. 
	'     * Winkler's paper used a default value of 0.7
	'     

	Private Shared ReadOnly mWeightThreshold As Double = 0.7

	' Size of the prefix to be concidered by the Winkler modification. 
	'     * Winkler's paper used a default value of 4
	'     

	Private Shared ReadOnly mNumChars As Integer = 4


	''' <summary>
	''' Returns the Jaro-Winkler distance between the specified  
	''' strings. The distance is symmetric and will fall in the 
	''' range 0 (perfect match) to 1 (no match). 
	''' </summary>
	''' <param name="aString1">First String</param>
	''' <param name="aString2">Second String</param>
	''' <returns></returns>
	Public Shared Function distance(ByVal aString1 As String, ByVal aString2 As String) As Double
		Return 1.0 - proximity(aString1, aString2)
	End Function


	''' <summary>
	''' Returns the Jaro-Winkler distance between the specified  
	''' strings. The distance is symmetric and will fall in the 
	''' range 0 (no match) to 1 (perfect match). 
	''' </summary>
	''' <param name="aString1">First String</param>
	''' <param name="aString2">Second String</param>
	''' <returns></returns>
	Public Shared Function proximity(ByVal aString1 As String, ByVal aString2 As String) As Double
		Dim lLen1 As Integer = aString1.Length
		Dim lLen2 As Integer = aString2.Length
		If lLen1 = 0 Then
			Return If(lLen2 = 0, 1.0, 0.0)
		End If

		Dim lSearchRange As Integer = Math.Max(0, Math.Max(lLen1, lLen2) / 2 - 1)

		Dim lMatched1 As Boolean() = New Boolean(lLen1 - 1) {}
		For i As Integer = 0 To lMatched1.Length - 1
			lMatched1(i) = False
		Next
		Dim lMatched2 As Boolean() = New Boolean(lLen2 - 1) {}
		For i As Integer = 0 To lMatched2.Length - 1
			lMatched2(i) = False
		Next

		Dim lNumCommon As Integer = 0
		For i As Integer = 0 To lLen1 - 1
			Dim lStart As Integer = Math.Max(0, i - lSearchRange)
			Dim lEnd As Integer = Math.Min(i + lSearchRange + 1, lLen2)
			For j As Integer = lStart To lEnd - 1
				If lMatched2(j) Then
					Continue For
				End If
				If aString1(i) <> aString2(j) Then
					Continue For
				End If
				lMatched1(i) = True
				lMatched2(j) = True
				lNumCommon += 1
				Exit For
			Next
		Next
		If lNumCommon = 0 Then
			Return 0.0
		End If

		Dim lNumHalfTransposed As Integer = 0
		Dim k As Integer = 0
		For i As Integer = 0 To lLen1 - 1
			If Not lMatched1(i) Then
				Continue For
			End If
			While Not lMatched2(k)
				k += 1
			End While
			If aString1(i) <> aString2(k) Then
				lNumHalfTransposed += 1
			End If
			k += 1
		Next
		' System.Diagnostics.Debug.WriteLine("numHalfTransposed=" + numHalfTransposed);
		Dim lNumTransposed As Integer = lNumHalfTransposed \ 2

		' System.Diagnostics.Debug.WriteLine("numCommon=" + numCommon + " numTransposed=" + numTransposed);
		Dim lNumCommonD As Double = lNumCommon
		Dim lWeight As Double = (lNumCommonD / lLen1 + lNumCommonD / lLen2 + (lNumCommon - lNumTransposed) / lNumCommonD) / 3.0

		If lWeight <= mWeightThreshold Then
			Return lWeight
		End If
		Dim lMax As Integer = Math.Min(mNumChars, Math.Min(aString1.Length, aString2.Length))
		Dim lPos As Integer = 0
		While lPos < lMax AndAlso aString1(lPos) = aString2(lPos)
			lPos += 1
		End While
		If lPos = 0 Then
			Return lWeight
		End If
		Return lWeight + 0.1 * lPos * (1.0 - lWeight)

	End Function


End Class
