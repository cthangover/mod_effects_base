# Effects Engine — efectos visuales animados para las escenas

Effects Engine permite que un escenario reproduzca sobre la escena un efecto visual breve: un destello, humo, chispas, un resplandor mágico. Un efecto es una pequeña escena de Godot que el mod lleva en su carpeta `effects/`; el escenario lo lanza por su nombre, y permanece en pantalla hasta que termina solo o se acaba el diálogo.

El motor también admite ambiente continuo: hojas, luciérnagas, nieve, polvo y lluvia. Village Pack aporta los perfiles de RPG Plus; otros mods pueden añadir perfiles y tipos de emisores. El anfitrión funciona fuera de mapas con otra proyección.

## Cómo aparece un efecto

1. Se ejecuta una línea del escenario, `effect <nombre>`.
2. Effects Engine busca la escena `effects/<nombre>.tscn` entre los mods cargados.
3. El efecto se coloca sobre la imagen de la escena —por encima del fondo y sus objetos, por debajo del cuadro de diálogo— y se reproduce.
4. Cuando termina el diálogo que lo lanzó, el efecto se retira, salvo que ya se haya retirado solo.

El escenario no espera al efecto: la línea siguiente se muestra enseguida.

## Para autores: añadir un efecto

Pon la escena del efecto en tu mod:

```filestree
my_mod/
└── effects/
    └── sparks.tscn
```

Lánzalo desde un escenario:

```scenario
text "La lámpara parpadea."
effect sparks
text "Algo se movió en la oscuridad."
end
```

**Los mods deben ser carpetas.** Las escenas de Godot solo se cargan desde un mod instalado como carpeta. En un mod distribuido como archivo `.zip` los efectos se omiten, con un aviso en el registro.

### Piezas para la escena del efecto

Effects Engine ofrece tres maneras de animar fotogramas dentro de la escena de tu efecto:

- **Conjunto de fotogramas con fundido** — un nodo con el script `EffectDataSet`, una lista de imágenes de fotogramas y un `AnimationController` que funde un fotograma con el siguiente. La velocidad y el tiempo de fundido se ajustan en el inspector; los fotogramas se reproducen una vez y el efecto se retira solo.
- **Hoja de sprites en un shader** — `EffectAnimator` avanza un `TextureRect` con material de hoja de fotogramas por un número fijo de fotogramas en una duración fija.
- **Hoja de sprites sin shader** — `EffectAnimNode` recorre una hoja de sprites horizontal (ancho de fotograma = ancho de la hoja / número de fotogramas) y se retira al final. También puede crearse desde código con `EffectAnimNode.SpawnOn(target, sheet, frameCount, seconds)`.

El conjunto de fotogramas y la hoja de sprites sin shader se estiran para llenar la escena, de modo que los fotogramas dibujados al tamaño de la escena encajan con el fondo.

### Un primer efecto sin código

El efecto más sencillo es una escena cuya raíz es un `TextureRect` con una imagen estirada a toda la pantalla — por ejemplo, un resplandor semitransparente. Aparece con `effect <nombre>` y desaparece cuando termina el diálogo.

## Si algo falla

1. **No aparece nada y el registro dice que el efecto no se encontró.** El archivo debe estar como `effects/<nombre>.tscn` en un mod que forme parte del juego, y el nombre del escenario debe coincidir.
2. **El efecto funciona en desarrollo pero no para los jugadores.** El mod se publicó como `.zip`; publícalo como carpeta.
3. **El efecto nunca se va.** Permanece hasta que termina su diálogo; cierra el diálogo, o monta el efecto con las piezas de conjunto de fotogramas o de hoja de sprites, que se retiran solas al acabar.

## Perfiles de ambiente continuo

El motor también admite ambiente continuo: hojas, luciérnagas, nieve, polvo y lluvia. Village Pack aporta los perfiles de RPG Plus; otros mods pueden añadir perfiles y tipos de emisores. El anfitrión funciona fuera de mapas con otra proyección.

```json
{
  "Kind": "atmosphere",
  "Items": [{
    "Id": "my_mod/ash",
    "Emitters": [{
      "Type": "particles", "Shape": "flake", "Count": 80,
      "Color": "#a0a0a070", "Velocity": [0.1, 0.02, -0.4],
      "Height": [0.1, 5], "Size": [0.5, 1.2], "Sway": 0.2, "Seed": 17
    }]
  }]
}
```

Los perfiles son entidades `atmosphere`. Selecciona su ID en `Atmosphere` del mapa. `particles` admite `flake`, `leaf`, `glow`, `dust`, `streak`; `ripples` dibuja anillos en una `Region` del anfitrión (el mundo RPG aporta `water`). Sin región no hay anillos.

`Velocity` usa XYZ en unidades del mundo por segundo, Z positivo hacia arriba; `Height` es un rango mundial y `Size` un radio en píxeles. `Pulse` regula el desvanecimiento y `Trail` la estela. Máximo 512 partículas por emisor y 2048 por perfil.

Para añadir tipos implementa `IAtmosphereEmitterFactory` de `Cthangover.Mods.Effects.Atmosphere`: `Id` identifica `Type`, `Create` recibe `AtmosphereEmitterInfo` con `ExtraFields`. El emisor implementa `Advance(delta, context)`, `Draw(canvas, context)` y `Dispose()`. Usa `context.Project`, `Bounds` y `Regions`. El registro sigue la carga y descarga del mod; el anfitrión libera sus emisores.

Fuera de mapas usa `AtmosphereView`: `Configure(profileId, context)`, `Advance(delta)` por fotograma y `Release()` antes de liberarlo manualmente. No requiere RPG.
