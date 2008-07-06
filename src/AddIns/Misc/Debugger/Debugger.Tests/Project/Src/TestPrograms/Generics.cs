﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision$</version>
// </file>

using System;

namespace Debugger.Tests.TestPrograms
{
	public class MainClass
	{
		public static void Main()
		{
			GenericClass<int, string> gClass = new GenericClass<int, string>();
			gClass.Metod(1, "1!");
			gClass.GenericMethod<bool>(2, "2!");
			GenericClass<int, string>.StaticMetod(3, "3!");
			GenericClass<int, string>.StaticGenericMethod<bool>(4, "4!");
			
			GenericStruct<int, string> gStruct = new GenericStruct<int, string>();
			gStruct.Metod(5, "5!");
			gStruct.GenericMethod<bool>(6, "6!");
			GenericStruct<int, string>.StaticMetod(7, "7!");
			GenericStruct<int, string>.StaticGenericMethod<bool>(8, "8!");
			
			System.Diagnostics.Debugger.Break();
		}
	}
	
	public class GenericClass<V, K>
	{
		public V Prop {
			get {
				return default(V);
			}
		}
		
		public static V StaticProp {
			get {
				return default(V);
			}
		}
		
		public K Metod(V v, K k)
		{
			System.Diagnostics.Debugger.Break();
			return k;
		}
		
		public T GenericMethod<T>(V v, K k)
		{
			System.Diagnostics.Debugger.Break();
			return default(T);
		}
		
		public static K StaticMetod(V v, K k)
		{
			System.Diagnostics.Debugger.Break();
			return k;
		}
		
		public static T StaticGenericMethod<T>(V v, K k)
		{
			System.Diagnostics.Debugger.Break();
			return default(T);
		}
	}
	
	public struct GenericStruct<V, K>
	{
		public K Metod(V v, K k)
		{
			System.Diagnostics.Debugger.Break();
			return k;
		}
		
		public T GenericMethod<T>(V v, K k)
		{
			System.Diagnostics.Debugger.Break();
			return default(T);
		}
		
		public static K StaticMetod(V v, K k)
		{
			System.Diagnostics.Debugger.Break();
			return k;
		}
		
		public static T StaticGenericMethod<T>(V v, K k)
		{
			System.Diagnostics.Debugger.Break();
			return default(T);
		}
	}
}

#if TEST_CODE
namespace Debugger.Tests {
	public partial class DebuggerTests
	{
		[NUnit.Framework.Test]
		public void Generics()
		{
			ExpandProperties(
				"StackFrame.MethodInfo",
				"MemberInfo.DeclaringType"
			);
			StartTest("Generics.cs");
			
			for(int i = 0; i < 8; i++) {
				ObjectDump("SelectedStackFrame", process.SelectedStackFrame);
				ObjectDump("SelectedStackFrame-GetArguments", process.SelectedStackFrame.GetArgumentValues());
				process.Continue();
			}
			ObjectDump("Prop", process.SelectedStackFrame.GetLocalVariableValue("gClass").GetMemberValue("Prop"));
			ObjectDump("StaticProp", process.SelectedStackFrame.GetLocalVariableValue("gClass").GetMemberValue("StaticProp"));
			
			EndTest();
		}
	}
}
#endif

#if EXPECTED_OUTPUT
<?xml version="1.0" encoding="utf-8"?>
<DebuggerTests>
  <Test
    name="Generics.cs">
    <ProcessStarted />
    <ModuleLoaded>mscorlib.dll (No symbols)</ModuleLoaded>
    <ModuleLoaded>Generics.exe (Has symbols)</ModuleLoaded>
    <DebuggingPaused>Break</DebuggingPaused>
    <SelectedStackFrame>
      <StackFrame
        ArgumentCount="2"
        Depth="0"
        HasSymbols="True"
        IsInvalid="False"
        MethodInfo="Metod"
        NextStatement="Generics.cs:48,4-48,40">
        <MethodInfo>
          <MethodInfo
            DeclaringType="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;"
            FullName="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;.Metod"
            IsInternal="False"
            IsPrivate="False"
            IsProtected="False"
            IsPublic="True"
            IsSpecialName="False"
            IsStatic="False"
            Module="Generics.exe"
            Name="Metod">
            <DeclaringType>
              <DebugType
                BaseType="System.Object"
                FullName="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;"
                HasElementType="False"
                Interfaces="System.Collections.Generic.List`1[Debugger.MetaData.DebugType]"
                IsArray="False"
                IsClass="True"
                IsGenericType="True"
                IsInteger="False"
                IsInterface="False"
                IsPrimitive="False"
                IsValueType="False"
                ManagedType="null"
                Module="Generics.exe" />
            </DeclaringType>
          </MethodInfo>
        </MethodInfo>
      </StackFrame>
    </SelectedStackFrame>
    <SelectedStackFrame-GetArguments
      Capacity="4"
      Count="2">
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="1"
          Expression="v"
          IsArray="False"
          IsInteger="True"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="1"
          Type="System.Int32" />
      </Item>
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="1!"
          Expression="k"
          IsArray="False"
          IsInteger="False"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="1!"
          Type="System.String" />
      </Item>
    </SelectedStackFrame-GetArguments>
    <DebuggingPaused>Break</DebuggingPaused>
    <SelectedStackFrame>
      <StackFrame
        ArgumentCount="2"
        Depth="0"
        HasSymbols="True"
        IsInvalid="False"
        MethodInfo="GenericMethod"
        NextStatement="Generics.cs:54,4-54,40">
        <MethodInfo>
          <MethodInfo
            DeclaringType="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;"
            FullName="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;.GenericMethod"
            IsInternal="False"
            IsPrivate="False"
            IsProtected="False"
            IsPublic="True"
            IsSpecialName="False"
            IsStatic="False"
            Module="Generics.exe"
            Name="GenericMethod">
            <DeclaringType>
              <DebugType
                BaseType="System.Object"
                FullName="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;"
                HasElementType="False"
                Interfaces="System.Collections.Generic.List`1[Debugger.MetaData.DebugType]"
                IsArray="False"
                IsClass="True"
                IsGenericType="True"
                IsInteger="False"
                IsInterface="False"
                IsPrimitive="False"
                IsValueType="False"
                ManagedType="null"
                Module="Generics.exe" />
            </DeclaringType>
          </MethodInfo>
        </MethodInfo>
      </StackFrame>
    </SelectedStackFrame>
    <SelectedStackFrame-GetArguments
      Capacity="4"
      Count="2">
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="2"
          Expression="v"
          IsArray="False"
          IsInteger="True"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="2"
          Type="System.Int32" />
      </Item>
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="2!"
          Expression="k"
          IsArray="False"
          IsInteger="False"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="2!"
          Type="System.String" />
      </Item>
    </SelectedStackFrame-GetArguments>
    <DebuggingPaused>Break</DebuggingPaused>
    <SelectedStackFrame>
      <StackFrame
        ArgumentCount="2"
        Depth="0"
        HasSymbols="True"
        IsInvalid="False"
        MethodInfo="StaticMetod"
        NextStatement="Generics.cs:60,4-60,40">
        <MethodInfo>
          <MethodInfo
            DeclaringType="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;"
            FullName="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;.StaticMetod"
            IsInternal="False"
            IsPrivate="False"
            IsProtected="False"
            IsPublic="True"
            IsSpecialName="False"
            IsStatic="True"
            Module="Generics.exe"
            Name="StaticMetod">
            <DeclaringType>
              <DebugType
                BaseType="System.Object"
                FullName="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;"
                HasElementType="False"
                Interfaces="System.Collections.Generic.List`1[Debugger.MetaData.DebugType]"
                IsArray="False"
                IsClass="True"
                IsGenericType="True"
                IsInteger="False"
                IsInterface="False"
                IsPrimitive="False"
                IsValueType="False"
                ManagedType="null"
                Module="Generics.exe" />
            </DeclaringType>
          </MethodInfo>
        </MethodInfo>
      </StackFrame>
    </SelectedStackFrame>
    <SelectedStackFrame-GetArguments
      Capacity="4"
      Count="2">
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="3"
          Expression="v"
          IsArray="False"
          IsInteger="True"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="3"
          Type="System.Int32" />
      </Item>
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="3!"
          Expression="k"
          IsArray="False"
          IsInteger="False"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="3!"
          Type="System.String" />
      </Item>
    </SelectedStackFrame-GetArguments>
    <DebuggingPaused>Break</DebuggingPaused>
    <SelectedStackFrame>
      <StackFrame
        ArgumentCount="2"
        Depth="0"
        HasSymbols="True"
        IsInvalid="False"
        MethodInfo="StaticGenericMethod"
        NextStatement="Generics.cs:66,4-66,40">
        <MethodInfo>
          <MethodInfo
            DeclaringType="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;"
            FullName="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;.StaticGenericMethod"
            IsInternal="False"
            IsPrivate="False"
            IsProtected="False"
            IsPublic="True"
            IsSpecialName="False"
            IsStatic="True"
            Module="Generics.exe"
            Name="StaticGenericMethod">
            <DeclaringType>
              <DebugType
                BaseType="System.Object"
                FullName="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;"
                HasElementType="False"
                Interfaces="System.Collections.Generic.List`1[Debugger.MetaData.DebugType]"
                IsArray="False"
                IsClass="True"
                IsGenericType="True"
                IsInteger="False"
                IsInterface="False"
                IsPrimitive="False"
                IsValueType="False"
                ManagedType="null"
                Module="Generics.exe" />
            </DeclaringType>
          </MethodInfo>
        </MethodInfo>
      </StackFrame>
    </SelectedStackFrame>
    <SelectedStackFrame-GetArguments
      Capacity="4"
      Count="2">
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="4"
          Expression="v"
          IsArray="False"
          IsInteger="True"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="4"
          Type="System.Int32" />
      </Item>
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="4!"
          Expression="k"
          IsArray="False"
          IsInteger="False"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="4!"
          Type="System.String" />
      </Item>
    </SelectedStackFrame-GetArguments>
    <DebuggingPaused>Break</DebuggingPaused>
    <SelectedStackFrame>
      <StackFrame
        ArgumentCount="2"
        Depth="0"
        HasSymbols="True"
        IsInvalid="False"
        MethodInfo="Metod"
        NextStatement="Generics.cs:75,4-75,40">
        <MethodInfo>
          <MethodInfo
            DeclaringType="Debugger.Tests.TestPrograms.GenericStruct&lt;System.Int32,System.String&gt;"
            FullName="Debugger.Tests.TestPrograms.GenericStruct&lt;System.Int32,System.String&gt;.Metod"
            IsInternal="False"
            IsPrivate="False"
            IsProtected="False"
            IsPublic="True"
            IsSpecialName="False"
            IsStatic="False"
            Module="Generics.exe"
            Name="Metod">
            <DeclaringType>
              <DebugType
                BaseType="System.ValueType"
                FullName="Debugger.Tests.TestPrograms.GenericStruct&lt;System.Int32,System.String&gt;"
                HasElementType="False"
                Interfaces="System.Collections.Generic.List`1[Debugger.MetaData.DebugType]"
                IsArray="False"
                IsClass="False"
                IsGenericType="True"
                IsInteger="False"
                IsInterface="False"
                IsPrimitive="False"
                IsValueType="True"
                ManagedType="null"
                Module="Generics.exe" />
            </DeclaringType>
          </MethodInfo>
        </MethodInfo>
      </StackFrame>
    </SelectedStackFrame>
    <SelectedStackFrame-GetArguments
      Capacity="4"
      Count="2">
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="5"
          Expression="v"
          IsArray="False"
          IsInteger="True"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="5"
          Type="System.Int32" />
      </Item>
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="5!"
          Expression="k"
          IsArray="False"
          IsInteger="False"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="5!"
          Type="System.String" />
      </Item>
    </SelectedStackFrame-GetArguments>
    <DebuggingPaused>Break</DebuggingPaused>
    <SelectedStackFrame>
      <StackFrame
        ArgumentCount="2"
        Depth="0"
        HasSymbols="True"
        IsInvalid="False"
        MethodInfo="GenericMethod"
        NextStatement="Generics.cs:81,4-81,40">
        <MethodInfo>
          <MethodInfo
            DeclaringType="Debugger.Tests.TestPrograms.GenericStruct&lt;System.Int32,System.String&gt;"
            FullName="Debugger.Tests.TestPrograms.GenericStruct&lt;System.Int32,System.String&gt;.GenericMethod"
            IsInternal="False"
            IsPrivate="False"
            IsProtected="False"
            IsPublic="True"
            IsSpecialName="False"
            IsStatic="False"
            Module="Generics.exe"
            Name="GenericMethod">
            <DeclaringType>
              <DebugType
                BaseType="System.ValueType"
                FullName="Debugger.Tests.TestPrograms.GenericStruct&lt;System.Int32,System.String&gt;"
                HasElementType="False"
                Interfaces="System.Collections.Generic.List`1[Debugger.MetaData.DebugType]"
                IsArray="False"
                IsClass="False"
                IsGenericType="True"
                IsInteger="False"
                IsInterface="False"
                IsPrimitive="False"
                IsValueType="True"
                ManagedType="null"
                Module="Generics.exe" />
            </DeclaringType>
          </MethodInfo>
        </MethodInfo>
      </StackFrame>
    </SelectedStackFrame>
    <SelectedStackFrame-GetArguments
      Capacity="4"
      Count="2">
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="6"
          Expression="v"
          IsArray="False"
          IsInteger="True"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="6"
          Type="System.Int32" />
      </Item>
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="6!"
          Expression="k"
          IsArray="False"
          IsInteger="False"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="6!"
          Type="System.String" />
      </Item>
    </SelectedStackFrame-GetArguments>
    <DebuggingPaused>Break</DebuggingPaused>
    <SelectedStackFrame>
      <StackFrame
        ArgumentCount="2"
        Depth="0"
        HasSymbols="True"
        IsInvalid="False"
        MethodInfo="StaticMetod"
        NextStatement="Generics.cs:87,4-87,40">
        <MethodInfo>
          <MethodInfo
            DeclaringType="Debugger.Tests.TestPrograms.GenericStruct&lt;System.Int32,System.String&gt;"
            FullName="Debugger.Tests.TestPrograms.GenericStruct&lt;System.Int32,System.String&gt;.StaticMetod"
            IsInternal="False"
            IsPrivate="False"
            IsProtected="False"
            IsPublic="True"
            IsSpecialName="False"
            IsStatic="True"
            Module="Generics.exe"
            Name="StaticMetod">
            <DeclaringType>
              <DebugType
                BaseType="System.ValueType"
                FullName="Debugger.Tests.TestPrograms.GenericStruct&lt;System.Int32,System.String&gt;"
                HasElementType="False"
                Interfaces="System.Collections.Generic.List`1[Debugger.MetaData.DebugType]"
                IsArray="False"
                IsClass="False"
                IsGenericType="True"
                IsInteger="False"
                IsInterface="False"
                IsPrimitive="False"
                IsValueType="True"
                ManagedType="null"
                Module="Generics.exe" />
            </DeclaringType>
          </MethodInfo>
        </MethodInfo>
      </StackFrame>
    </SelectedStackFrame>
    <SelectedStackFrame-GetArguments
      Capacity="4"
      Count="2">
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="7"
          Expression="v"
          IsArray="False"
          IsInteger="True"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="7"
          Type="System.Int32" />
      </Item>
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="7!"
          Expression="k"
          IsArray="False"
          IsInteger="False"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="7!"
          Type="System.String" />
      </Item>
    </SelectedStackFrame-GetArguments>
    <DebuggingPaused>Break</DebuggingPaused>
    <SelectedStackFrame>
      <StackFrame
        ArgumentCount="2"
        Depth="0"
        HasSymbols="True"
        IsInvalid="False"
        MethodInfo="StaticGenericMethod"
        NextStatement="Generics.cs:93,4-93,40">
        <MethodInfo>
          <MethodInfo
            DeclaringType="Debugger.Tests.TestPrograms.GenericStruct&lt;System.Int32,System.String&gt;"
            FullName="Debugger.Tests.TestPrograms.GenericStruct&lt;System.Int32,System.String&gt;.StaticGenericMethod"
            IsInternal="False"
            IsPrivate="False"
            IsProtected="False"
            IsPublic="True"
            IsSpecialName="False"
            IsStatic="True"
            Module="Generics.exe"
            Name="StaticGenericMethod">
            <DeclaringType>
              <DebugType
                BaseType="System.ValueType"
                FullName="Debugger.Tests.TestPrograms.GenericStruct&lt;System.Int32,System.String&gt;"
                HasElementType="False"
                Interfaces="System.Collections.Generic.List`1[Debugger.MetaData.DebugType]"
                IsArray="False"
                IsClass="False"
                IsGenericType="True"
                IsInteger="False"
                IsInterface="False"
                IsPrimitive="False"
                IsValueType="True"
                ManagedType="null"
                Module="Generics.exe" />
            </DeclaringType>
          </MethodInfo>
        </MethodInfo>
      </StackFrame>
    </SelectedStackFrame>
    <SelectedStackFrame-GetArguments
      Capacity="4"
      Count="2">
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="8"
          Expression="v"
          IsArray="False"
          IsInteger="True"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="8"
          Type="System.Int32" />
      </Item>
      <Item>
        <Value
          ArrayDimensions="{Exception: Value is not an array}"
          ArrayLenght="{Exception: Value is not an array}"
          ArrayRank="{Exception: Value is not an array}"
          AsString="8!"
          Expression="k"
          IsArray="False"
          IsInteger="False"
          IsInvalid="False"
          IsNull="False"
          IsObject="False"
          IsPrimitive="True"
          PrimitiveValue="8!"
          Type="System.String" />
      </Item>
    </SelectedStackFrame-GetArguments>
    <DebuggingPaused>Break</DebuggingPaused>
    <Prop>
      <Value
        ArrayDimensions="{Exception: Value is not an array}"
        ArrayLenght="{Exception: Value is not an array}"
        ArrayRank="{Exception: Value is not an array}"
        AsString="0"
        Expression="gClass.Prop"
        IsArray="False"
        IsInteger="True"
        IsInvalid="False"
        IsNull="False"
        IsObject="False"
        IsPrimitive="True"
        PrimitiveValue="0"
        Type="System.Int32" />
    </Prop>
    <StaticProp>
      <Value
        ArrayDimensions="{Exception: Value is not an array}"
        ArrayLenght="{Exception: Value is not an array}"
        ArrayRank="{Exception: Value is not an array}"
        AsString="0"
        Expression="Debugger.Tests.TestPrograms.GenericClass&lt;System.Int32,System.String&gt;.StaticProp"
        IsArray="False"
        IsInteger="True"
        IsInvalid="False"
        IsNull="False"
        IsObject="False"
        IsPrimitive="True"
        PrimitiveValue="0"
        Type="System.Int32" />
    </StaticProp>
    <ProcessExited />
  </Test>
</DebuggerTests>
#endif // EXPECTED_OUTPUT