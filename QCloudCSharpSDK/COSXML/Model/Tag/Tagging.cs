using System.Collections.Generic;

namespace COSXML.Model.Tag {
  public sealed class Tagging {

    public readonly TagSet tagSet;

    public Tagging() {
      this.tagSet = new TagSet();
    }

    public void AddTag(string key, string value) {
      this.tagSet.tags.Add(new Tag(key, value));
    }

    public sealed class TagSet {
      public readonly List<Tag> tags;

      public TagSet() {
        this.tags = new List<Tag>();
      }
    }

    public sealed class Tag {
      public readonly string key;

      public readonly string value;

      public Tag(string key, string value) {
        this.key = key;
        this.value = value;
      }
    }
  }
}