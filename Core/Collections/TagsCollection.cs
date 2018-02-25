/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria.Collections                              License  : Please read LICENSE.txt file      *
*  Type      : TagsCollection                                   Pattern  : Collection                        *
*                                                                                                            *
*  Summary   : Collection of string tags.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Empiria.Collections {

  /// <summary>Collection of string tags.</summary>
  public class TagsCollection {

    #region Fields

    private List<string> tagsArray = new List<string>(4);

    #endregion Fields

    #region Constructors and parsers

    public TagsCollection() {
      // no-op
    }


    private TagsCollection(string tagsString) {
      if (tagsString == null) {
        tagsString = String.Empty;
      }
      this.Load(tagsString);
    }


    private TagsCollection(IList<string> tagsArray) {
      Assertion.AssertObject(tagsArray, "tagsArray");

      this.tagsArray = tagsArray.ToList();
    }


    static public TagsCollection Parse(string tagsString) {
      return new TagsCollection(tagsString);
    }


    static public TagsCollection Parse(IList<string> tagsArray) {
      return new TagsCollection(tagsArray);
    }


    static public TagsCollection Empty {
      get {
        return new TagsCollection(String.Empty);
      }
    }

    #endregion Constructors and parsers

    #region Properties

    public int Count {
      get {
        return this.tagsArray.Count;
      }
    }


    public IList<string> Items {
      get {
         return this.tagsArray;
      }
    }

    #endregion Properties

    #region Public methods

    public void Add(string tag) {
      Assertion.AssertObject(tag, "tag");

      tag = CleanTag(tag);

      if (!this.tagsArray.Contains(tag)) {
        this.tagsArray.Add(tag);
      }
    }


    public void AddRange(TagsCollection tags) {
      Assertion.AssertObject(tags, "tags");

      foreach (var tag in tags.Items) {
        this.tagsArray.Add(tag);
      }
    }


    public bool Contains(string tag) {
      Assertion.AssertObject(tag, "tag");

      tag = CleanTag(tag);

      return this.tagsArray.Contains(tag);
    }


    public bool Remove(string tag) {
      Assertion.AssertObject(tag, "tag");

      tag = CleanTag(tag);

      return this.tagsArray.Remove(tag);
    }


    public void Sort() {
      this.tagsArray.Sort();
    }


    public override string ToString() {
      var temp = String.Empty;

      foreach (var item in this.tagsArray) {
        temp += $"'{item}' ";
      }

      return EmpiriaString.TrimAll(temp);
    }

    #endregion Public methods

    #region Private methods

    private string CleanTag(string tag) {
      tag = tag.Replace("'", "");
      tag = EmpiriaString.TrimAll(tag);

      return tag;
    }


    private void Load(string tagsString) {
      tagsString = EmpiriaString.TrimAll(tagsString);


      string pattern = @"'([^']*)";

      foreach (string value in Regex.Split(tagsString, pattern)) {

        var temp = CleanTag(value);

        if (temp.Length != 0 && !this.tagsArray.Contains(temp)) {
          this.tagsArray.Add(temp);
        }
      }
    }

    #endregion Private methods

  } // class TagsCollection

} // namespace Empiria.Collections
