using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using IG.Lib;
using System.IO;

namespace IG.Tests
{
    
    
    public class IGLibCoreReflectionTests
    {

        [Test]
        public void IEnumerableElementTypeIsRetrievedCorrectlyForIEnumerableOfDifferentTypes()
        {
            // IEnumerable of value types:
            CheckRetrievedElementTypeForIEnumerable<int>();
            CheckRetrievedElementTypeForIEnumerable<string>();
            CheckRetrievedElementTypeForIEnumerable<FileInfo>();
            // IEnumerable of arrays>
            CheckRetrievedElementTypeForIEnumerable<double[]>();
            CheckRetrievedElementTypeForIEnumerable<object[]>();
            // IEnumerable of generic collections:
            CheckRetrievedElementTypeForIEnumerable<Dictionary<int, double>>();
            CheckRetrievedElementTypeForIEnumerable<List<DirectoryInfo>>();
        }

        public void CheckRetrievedElementTypeForIEnumerable<ElementType>()
        {
            Type IEnumerableType = typeof(IEnumerable<ElementType>);
            Type elementType = UtilReflectionCore.GetEnumerableType(IEnumerableType);
            Type expectedElementType = typeof(ElementType);
            Assert.AreEqual(expectedElementType, elementType);
        }


        [Test]
        public void IEnumerableElementTypeIsRetrievedCorrectlyForGenericCollections()
        {
            CheckRetrievedElementTypeForGenericCollectionOrArray<List<double>, double>();
            CheckRetrievedElementTypeForGenericCollectionOrArray<List<string>, string>();
            CheckRetrievedElementTypeForGenericCollectionOrArray<List<FileInfo>, FileInfo>();

            CheckRetrievedElementTypeForGenericCollectionOrArray<Stack<double>, double>();
            CheckRetrievedElementTypeForGenericCollectionOrArray<Stack<string>, string>();
            CheckRetrievedElementTypeForGenericCollectionOrArray<Stack<FileInfo>, FileInfo>();

            CheckRetrievedElementTypeForGenericCollectionOrArray<Dictionary<int, double>, KeyValuePair<int, double>>();
        }

        [Test]
        public void IEnumerableElementTypeIsRetrievedCorrectlyForGenericCollectionInterfaces()
        {
            CheckRetrievedElementTypeForGenericCollectionOrArray<IList<double>, double>();
            CheckRetrievedElementTypeForGenericCollectionOrArray<IList<FileInfo>, FileInfo>();

            CheckRetrievedElementTypeForGenericCollectionOrArray <IList < List<double>>, List<double>>();
            CheckRetrievedElementTypeForGenericCollectionOrArray<IList<List<FileInfo>>, List<FileInfo>>();


            CheckRetrievedElementTypeForGenericCollectionOrArray<IReadOnlyCollection<double>, double>();
            CheckRetrievedElementTypeForGenericCollectionOrArray<IReadOnlyCollection<FileInfo>, FileInfo>();

            CheckRetrievedElementTypeForGenericCollectionOrArray<IDictionary<int, double>, KeyValuePair<int, double>>();
            CheckRetrievedElementTypeForGenericCollectionOrArray<IDictionary<int, FileInfo>, KeyValuePair<int, FileInfo>>();

            CheckRetrievedElementTypeForGenericCollectionOrArray<IReadOnlyDictionary<int, double>, KeyValuePair<int, double>>();
            CheckRetrievedElementTypeForGenericCollectionOrArray<IReadOnlyDictionary<int, FileInfo>, KeyValuePair<int, FileInfo>>();
        }

        [Test]
        public void IEnumerableElementTypeIsRetrievedCorrectlyForArrays()
        {
            CheckRetrievedElementTypeForGenericCollectionOrArray<double[], double>();
            CheckRetrievedElementTypeForGenericCollectionOrArray<string[], string>();
            CheckRetrievedElementTypeForGenericCollectionOrArray<FileInfo[], FileInfo>();

            CheckRetrievedElementTypeForGenericCollectionOrArray<Stack<double>[], Stack<double>>();
            CheckRetrievedElementTypeForGenericCollectionOrArray<IList<double>[], IList<double>>();
        }


        public void CheckRetrievedElementTypeForGenericCollectionOrArray<CollectionType, ElementType>()
        {
            Type IEnumerableType = typeof(CollectionType);
            Type retrievedElementType = UtilReflectionCore.GetEnumerableType(IEnumerableType);
            Type expectedElementType = typeof(ElementType);
            Assert.AreEqual(expectedElementType, retrievedElementType);
        }



        [Test]
        public void IEnumerableElementTypeIsRetrievedCorrectlyForNestedGenericCollections()
        {
            CheckRetrievedElementTypeForNestedGenericCollectionOrArray<IList<IList<double>>, double>();
            CheckRetrievedElementTypeForNestedGenericCollectionOrArray<IList<IList<FileInfo>>, FileInfo>();
            CheckRetrievedElementTypeForNestedGenericCollectionOrArray<Stack<List<double>>, double>();
            CheckRetrievedElementTypeForNestedGenericCollectionOrArray<Stack<List<FileInfo>>, FileInfo>();
        }

        [Test]
        public void IEnumerableElementTypeIsRetrievedCorrectlyForNestedArrays()
        {
            CheckRetrievedElementTypeForNestedGenericCollectionOrArray<double[][], double>();
            CheckRetrievedElementTypeForNestedGenericCollectionOrArray<string[][], string>();
            CheckRetrievedElementTypeForNestedGenericCollectionOrArray<IList<double>[][], IList<double>>();
        }

        [Test]
        public void IEnumerableElementTypeIsRetrievedCorrectlyForNestedGenericCollectionsOfArrays()
        {
            CheckRetrievedElementTypeForNestedGenericCollectionOrArray<List<double[]>, double>();
            CheckRetrievedElementTypeForNestedGenericCollectionOrArray<List<string[]>, string>();
        }

        [Test]
        public void IEnumerableElementTypeIsRetrievedCorrectlyForNestedArraysOfGenericCollections()
        {
            CheckRetrievedElementTypeForNestedGenericCollectionOrArray<List<double>[], double>();
            CheckRetrievedElementTypeForNestedGenericCollectionOrArray<List<string>[], string>();
        }

        public void CheckRetrievedElementTypeForNestedGenericCollectionOrArray<CollectionType, ElementType>()
        {
            Type IEnumerableType = typeof(CollectionType);
            Type retrievedOuterElementType = UtilReflectionCore.GetEnumerableType(IEnumerableType);
            Type retrievedElementType = UtilReflectionCore.GetEnumerableType(retrievedOuterElementType);
            Type expectedElementType = typeof(ElementType);
            Assert.AreEqual(expectedElementType, retrievedElementType);
        }


    }

}
