<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.9" tiledversion="1.9.2" name="mario" tilewidth="24" tileheight="32" tilecount="8" columns="4">
 <image source="mario.png" width="96" height="64"/>
 <tile id="0">
  <properties>
   <property name="MovingLeft" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" class="CollisionRectangle" x="0" y="0" width="24" height="32">
    <properties>
     <property name="Collision" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="1" duration="120"/>
   <frame tileid="2" duration="120"/>
  </animation>
 </tile>
 <tile id="1">
  <objectgroup draworder="index" id="2">
   <object id="1" class="CollisionRectangle" x="0" y="0" width="24" height="32">
    <properties>
     <property name="Collision" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="6">
  <properties>
   <property name="MovingRight" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="4">
   <object id="3" class="CollisionRectangle" x="0" y="0" width="24" height="32">
    <properties>
     <property name="Collision" type="bool" value="true"/>
    </properties>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="5" duration="120"/>
   <frame tileid="4" duration="120"/>
  </animation>
 </tile>
</tileset>
