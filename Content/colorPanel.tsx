<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.9" tiledversion="1.9.2" name="colorPanel" tilewidth="32" tileheight="32" tilecount="12" columns="12">
 <image source="colorPanel.png" width="384" height="32"/>
 <tile id="0">
  <properties>
   <property name="Collision" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="20" x="0" y="0" width="32" height="32"/>
  </objectgroup>
 </tile>
 <tile id="1">
  <properties>
   <property name="Trigger" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0">
    <properties>
     <property name="Trigger" type="bool" value="true"/>
    </properties>
    <polygon points="0,0 0,32 32,32 32,0"/>
   </object>
  </objectgroup>
 </tile>
</tileset>
