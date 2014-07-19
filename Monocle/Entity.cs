﻿using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Monocle
{
    public class Entity : IEnumerable<Component>, IEnumerable
    {
        public bool Active = true;
        public bool Visible = true;
        public bool Collidable = true;
        public Vector2 Position;

        public Scene Scene { get; private set; }
        public ComponentList Components { get; private set; }
        public List<int> Tags { get; private set; }

        private Collider collider;
        internal int depth = 0;
        internal float actualDepth = 0;

        public Entity(Vector2 position)
        {
            Position = position;

            Components = new ComponentList(this);
            Tags = new List<int>();
        }

        public Entity()
            : this(Vector2.Zero)
        {

        }

        public virtual void SceneBegin()
        {

        }

        public virtual void SceneEnd()
        {

        }

        public virtual void Added(Scene scene)
        {
            Scene = scene;
            foreach (var tag in Tags)
                Scene.TagEntity(tag, this);
            if (Components != null)
                foreach (var c in Components)
                    c.EntityAdded();
            Scene.SetActualDepth(this);
        }

        public virtual void Removed(Scene scene)
        {
            if (Components != null)
                foreach (var c in Components)
                    c.EntityRemoved();
            foreach (var tag in Tags)
                Scene.TagLists[tag].Remove(this);
            Scene = null;
        }

        public virtual void Update()
        {
            Components.Update();
        }

        public virtual void Render()
        {
            Components.Render();
        }

        public virtual void DebugRender()
        {
            if (Collider != null)
                Collider.Render(Collidable ? Color.Red : Color.DarkRed);
        }

        public virtual void HandleGraphicsReset()
        {
            Components.HandleGraphicsReset();
        }

        public void RemoveSelf()
        {
            if (Scene != null)
                Scene.Entities.Remove(this);
        }

        public int Depth
        {
            get { return depth; }
            set
            {
                if (depth != value)
                {
                    depth = value;
                    if (Scene != null)
                        Scene.SetActualDepth(this);
                }
            }
        }

        public float X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }

        public float Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }

        #region Collider

        public Collider Collider
        {
            get { return collider; }
            set
            {
                if (value == collider)
                    return;
#if DEBUG
                if (value.Entity != null)
                    throw new Exception("Setting an Entity's Collider to a Collider already in use by another Entity");
#endif
                if (collider != null)
                    collider.Removed();
                collider = value;
                if (collider != null)
                    collider.Added(this);
            }
        }

        public float Width
        {
            get
            {
                if (Collider == null)
                    return 0;
                else
                    return Collider.Width;
            }
        }

        public float Height
        {
            get
            {
                if (Collider == null)
                    return 0;
                else
                    return Collider.Height;
            }
        }

        public float Left
        {
            get
            {
                if (Collider == null)
                    return X;
                else
                    return Position.X + Collider.Left;
            }

            set
            {
                if (Collider == null)
                    Position.X = value;
                else
                    Position.X = value - Collider.Left;
            }
        }

        public float Right
        {
            get
            {
                if (Collider == null)
                    return Position.X;
                else
                    return Position.X + Collider.Right;
            }

            set
            {
                if (Collider == null)
                    Position.X = value;
                else
                    Position.X = value - Collider.Right;
            }
        }

        public float Top
        {
            get
            {
                if (Collider == null)
                    return Position.Y;
                else
                    return Position.Y + Collider.Top;
            }

            set
            {
                if (Collider == null)
                    Position.Y = value;
                else
                    Position.Y = value - Collider.Top;
            }
        }

        public float Bottom
        {
            get
            {
                if (Collider == null)
                    return Position.Y;
                else
                    return Position.Y + Collider.Bottom;
            }

            set
            {
                if (Collider == null)
                    Position.Y = value;
                else
                    Position.Y = value - Collider.Bottom;
            }
        }

        public float CenterX
        {
            get
            {
                if (Collider == null)
                    return Position.X;
                else
                    return Position.X + Collider.Left + Collider.Width / 2f;
            }

            set
            {
                if (Collider == null)
                    Position.X = value;
                else
                    Position.X = value - (Collider.Left + Collider.Width / 2f);
            }
        }

        public float CenterY
        {
            get
            {
                if (Collider == null)
                    return Position.Y;
                else
                    return Position.Y + Collider.Top + Collider.Height / 2f;
            }

            set
            {
                if (Collider == null)
                    Position.Y = value;
                else
                    Position.Y = value - (Collider.Top + Collider.Height / 2f);
            }
        }

        public Vector2 TopLeft
        {
            get
            {
                return new Vector2(Left, Top);
            }

            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }

        public Vector2 TopRight
        {
            get
            {
                return new Vector2(Right, Top);
            }

            set
            {
                Right = value.X;
                Top = value.Y;
            }
        }

        public Vector2 BottomLeft
        {
            get
            {
                return new Vector2(Left, Bottom);
            }

            set
            {
                Left = value.X;
                Bottom = value.Y;
            }
        }

        public Vector2 BottomRight
        {
            get
            {
                return new Vector2(Right, Bottom);
            }

            set
            {
                Right = value.X;
                Bottom = value.Y;
            }
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(CenterX, CenterY);
            }

            set
            {
                CenterX = value.X;
                CenterY = value.Y;
            }
        }

        public Vector2 CenterLeft
        {
            get
            {
                return new Vector2(Left, CenterY);
            }

            set
            {
                Left = value.X;
                CenterY = value.Y;
            }
        }

        public Vector2 CenterRight
        {
            get
            {
                return new Vector2(Right, CenterY);
            }

            set
            {
                Right = value.X;
                CenterY = value.Y;
            }
        }

        public Vector2 TopCenter
        {
            get
            {
                return new Vector2(CenterX, Top);
            }

            set
            {
                CenterX = value.X;
                Top = value.Y;
            }
        }

        public Vector2 BottomCenter
        {
            get
            {
                return new Vector2(CenterX, Bottom);
            }

            set
            {
                CenterX = value.X;
                Bottom = value.Y;
            }
        }

        #endregion

        #region Tags

        public void Tag(int tag)
        {
            if (!Tags.Contains(tag))
            {
                Tags.Add(tag);
                if (Scene != null)
                    Scene.TagEntity(tag, this);
            }
        }

        public void Tag(params int[] tags)
        {
            foreach (var tag in tags)
                Tag(tag);
        }

        public void Untag(int tag)
        {
            if (Tags.Contains(tag))
            {
                Tags.Add(tag);
                if (Scene != null)
                    Scene.TagLists[tag].Remove(this);
            }
        }

        public void Untag(params int[] tags)
        {
            foreach (var tag in tags)
                Untag(tag);
        }

        #endregion

        #region Collision Shortcuts

        #region Collide Check

        public bool CollideCheck(Entity other)
        {
            return Collide.Check(this, other);
        }

        public bool CollideCheck(Entity other, float atX, float atY)
        {
            return Collide.Check(this, other, atX, atY);
        }

        public bool CollideCheck(Entity other, Vector2 at)
        {
            return Collide.Check(this, other, at);
        }

        public bool CollideCheck(Vector2 point)
        {
            return Collide.Check(this, point);
        }

        public bool CollideCheck(Vector2 point, float atX, float atY)
        {
            return Collide.Check(this, point, atX, atY);
        }

        public bool CollideCheck(Vector2 point, Vector2 at)
        {
            return Collide.Check(this, point, at);
        }

        public bool CollideCheck(Rectangle rect)
        {
            return Collide.Check(this, rect);
        }

        public bool CollideCheck(Rectangle rect, Vector2 at)
        {
            return Collide.Check(this, rect, at);
        }

        public bool CollideCheck(Rectangle rect, float atX, float atY)
        {
            return Collide.Check(this, rect, atX, atY);
        }

        public bool CollideCheck(int tag)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif
            return Collide.Check(this, Scene[tag]);
        }

        public bool CollideCheck(int tag, float atX, float atY)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif
            return Collide.Check(this, Scene[tag], atX, atY);
        }

        public bool CollideCheck(int tag, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif
            return Collide.Check(this, Scene[tag], at);
        }

        public bool CollideCheck(int[] tags)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against tag lists when it is not a member of a Scene");
#endif
            foreach (var tag in tags)
                if (Collide.Check(this, Scene[tag]))
                    return true;
            return false;
        }

        public bool CollideCheck(int[] tags, float atX, float atY)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against tag lists when it is not a member of a Scene");
#endif
            foreach (var tag in tags)
                if (Collide.Check(this, Scene[tag], atX, atY))
                    return true;
            return false;
        }

        public bool CollideCheck(int[] tags, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against tag lists when it is not a member of a Scene");
#endif
            foreach (var tag in tags)
                if (Collide.Check(this, Scene[tag], at))
                    return true;
            return false;
        }

        #endregion

        #region Collide CheckOutside

        public bool CollideCheckOutside(Entity other, float atX, float atY)
        {
            return !Collide.Check(this, other) && Collide.Check(this, other, atX, atY);
        }

        public bool CollideCheckOutside(Entity other, Vector2 at)
        {
            return !Collide.Check(this, other) && Collide.Check(this, other, at);
        }

        public bool CollideCheckOutside(int tag, float atX, float atY)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif

            foreach (var entity in Scene[tag])
            {
                if (!Collide.Check(this, entity) && Collide.Check(this, entity, atX, atY))
                    return true;
            }

            return false;
        }

        public bool CollideCheckOutside(int tag, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif

            foreach (var entity in Scene[tag])
            {
                if (!Collide.Check(this, entity) && Collide.Check(this, entity, at))
                    return true;
            }

            return false;
        }

        #endregion

        #region Collide First

        public Entity CollideFirst(int tag)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif
            return Collide.First(this, Scene[tag]);
        }

        public Entity CollideFirst(int tag, float atX, float atY)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif
            return Collide.First(this, Scene[tag], atX, atY);
        }

        public Entity CollideFirst(int tag, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif
            return Collide.First(this, Scene[tag], at);
        }

        public Entity CollideFirst(int[] tags)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against tag lists when it is not a member of a Scene");
#endif
            Entity hit;

            foreach (var tag in tags)
                if ((hit = Collide.First(this, Scene[tag])) != null)
                    return hit;
            return null;
        }

        public Entity CollideFirst(int[] tags, float atX, float atY)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against tag lists when it is not a member of a Scene");
#endif
            Entity hit;

            foreach (var tag in tags)
                if ((hit = Collide.First(this, Scene[tag], atX, atY)) != null)
                    return hit;
            return null;
        }

        public Entity CollideFirst(int[] tags, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against tag lists when it is not a member of a Scene");
#endif
            Entity hit;

            foreach (var tag in tags)
                if ((hit = Collide.First(this, Scene[tag], at)) != null)
                    return hit;
            return null;
        }

        #endregion

        #region Collide FirstOutside

        public Entity CollideFirstOutside(int tag, float atX, float atY)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif

            foreach (var entity in Scene[tag])
            {
                if (!Collide.Check(this, entity) && Collide.Check(this, entity, atX, atY))
                    return entity;
            }

            return null;
        }

        public Entity CollideFirstOutside(int tag, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif

            foreach (var entity in Scene[tag])
            {
                if (!Collide.Check(this, entity) && Collide.Check(this, entity, at))
                    return entity;
            }

            return null;
        }

        #endregion

        #region Collide All

        public List<Entity> CollideAll(int tag)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif
            return Collide.All(this, Scene[tag]);
        }

        public List<Entity> CollideAll(int tag, float atX, float atY)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif
            return Collide.All(this, Scene[tag], atX, atY);
        }

        public List<Entity> CollideAll(int tag, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif
            return Collide.All(this, Scene[tag], at);
        }

        public List<Entity> CollideAll(int[] tags)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against tag lists when it is not a member of a Scene");
#endif
            List<Entity> list = new List<Entity>();
            foreach (var tag in tags)
                Collide.All(this, Scene[tag], list);
            return list;
        }

        public List<Entity> CollideAll(int[] tags, float atX, float atY)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against tag lists when it is not a member of a Scene");
#endif
            List<Entity> list = new List<Entity>();
            foreach (var tag in tags)
                Collide.All(this, Scene[tag], list, atX, atY);
            return list;
        }

        public List<Entity> CollideAll(int[] tags, Vector2 at)
        {
#if DEBUG
            if (Scene == null)
                throw new Exception("Can't collide check an Entity against tag lists when it is not a member of a Scene");
#endif
            List<Entity> list = new List<Entity>();
            foreach (var tag in tags)
                Collide.All(this, Scene[tag], list, at);
            return list;
        }

        #endregion

        public bool CollideDo(int tag, Action<Entity> action)
        {
            bool hit = false;
            foreach (var other in Scene[tag])
            {
                if (CollideCheck(other))
                {
                    action(other);
                    hit = true;
                }
            }
            return hit;
        }

        public bool CollideLine(Vector2 from, Vector2 to)
        {
            return Collider.Collide(from, to);
        }

        #endregion

        #region Components Shortcuts

        /// <summary>
        /// Shortcut function for adding a Component to the Entity's Components list
        /// </summary>
        /// <param name="component">The Component to add</param>
        public void Add(Component component)
        {
            Components.Add(component);
        }

        /// <summary>
        /// Shortcut function for removing an Component from the Entity's Components list
        /// </summary>
        /// <param name="component">The Component to remove</param>
        public void Remove(Component component)
        {
            Components.Remove(component);
        }

        /// <summary>
        /// Shortcut function for adding a set of Components from the Entity's Components list
        /// </summary>
        /// <param name="components">The Components to add</param>
        public void Add(params Component[] components)
        {
            Components.Add(components);
        }

        /// <summary>
        /// Shortcut function for removing a set of Components from the Entity's Components list
        /// </summary>
        /// <param name="components">The Components to remove</param>
        public void Remove(params Component[] components)
        {
            Components.Remove(components);
        }

        /// <summary>
        /// Allows you to iterate through all Components in the Entity
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Component> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        /// <summary>
        /// Allows you to iterate through all Components in the Entity
        /// </summary>
        /// <returns></returns>
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Misc Utils

        public Entity Closest(params Entity[] entities)
        {
            Entity closest = entities[0];
            float dist = Vector2.DistanceSquared(Position, closest.Position);

            for (int i = 1; i < entities.Length; i++)
            {
                float current = Vector2.DistanceSquared(Position, entities[i].Position);
                if (current < dist)
                {
                    closest = entities[i];
                    dist = current;
                }
            }

            return closest;
        }

        public Entity Closest(int tag)
        {
            var list = Scene[tag];
            Entity closest = null;
            float dist;

            if (list.Count >= 1)
            {
                closest = list[0];
                dist = Vector2.DistanceSquared(Position, closest.Position);

                for (int i = 1; i < list.Count; i++)
                {
                    float current = Vector2.DistanceSquared(Position, list[i].Position);
                    if (current < dist)
                    {
                        closest = list[i];
                        dist = current;
                    }
                }
            }

            return closest;
        }

        #endregion
    }
}
