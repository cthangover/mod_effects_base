# Effects Engine — efectos visuales animados para las escenas

Effects Engine permite que un escenario reproduzca sobre la escena un efecto visual breve: un destello, humo, chispas, un resplandor mágico. Un efecto es una pequeña escena de Godot que el mod lleva en su carpeta `effects/`; el escenario lo lanza por su nombre, y permanece en pantalla hasta que termina solo o se acaba el diálogo.

Es un mod de motor sin nada propio que ver. Los juegos incluidos por ahora no traen ninguna escena de efecto, así que no te toparás con uno mientras juegas; este mod está aquí para los autores que quieran añadir uno.

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
