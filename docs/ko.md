# Effects Engine — 장면을 위한 애니메이션 시각 효과

Effects Engine은 시나리오가 장면 위에 짧은 시각 효과를 재생할 수 있게 합니다. 번쩍임, 연기, 불똥, 마법의 빛 따위입니다. 효과란 모드가 자기 `effects/` 폴더에 넣어 배포하는 작은 Godot 장면입니다. 시나리오가 이름으로 불러내면 화면에 나타나고, 스스로 끝나거나 대화가 끝날 때까지 머뭅니다.

낙엽, 반딧불, 눈, 먼지, 비 같은 지속 환경 효과도 지원합니다. RPG Plus 프로필은 Village Pack에 있으며 다른 모드가 프로필과 방출기 유형을 추가할 수 있습니다. 좌표 투영을 제공하면 타일 맵 밖에서도 작동합니다.

## 효과는 이렇게 나타납니다

1. 시나리오 줄 `effect <이름>`이 실행됩니다.
2. Effects Engine이 로드된 모드들에서 `effects/<이름>.tscn` 장면을 찾습니다.
3. 효과는 장면 그림 위, 즉 배경과 그 물건들보다는 위, 대화 상자보다는 아래에 놓이고 재생됩니다.
4. 그 효과를 부른 대화가 끝나면 효과도 제거됩니다. 먼저 스스로 사라졌다면 그대로입니다.

시나리오는 효과를 기다리지 않습니다. 다음 줄이 곧바로 표시됩니다.

## 제작자를 위해: 효과 추가하기

효과 장면을 모드에 넣으세요:

```filestree
my_mod/
└── effects/
    └── sparks.tscn
```

시나리오에서 실행합니다:

```scenario
text "램프가 깜빡였다."
effect sparks
text "어둠 속에서 무언가 움직였다."
end
```

**모드는 폴더여야 합니다.** Godot 장면은 폴더로 설치된 모드에서만 불러올 수 있습니다. `.zip` 압축파일로 배포된 모드의 효과는 건너뛰어지며 로그에 경고가 남습니다.

### 효과 장면의 조각

Effects Engine은 효과 장면 안에서 프레임을 애니메이션하는 방법 세 가지를 제공합니다:

- **크로스페이드가 있는 프레임 세트** — `EffectDataSet` 스크립트를 단 노드, 프레임 그림 목록, 프레임에서 프레임으로 서서히 넘어가는 `AnimationController`. 속도와 페이드 시간은 인스펙터에서 정합니다. 프레임은 한 번 재생되고 효과는 스스로 사라집니다.
- **셰이더 속 스프라이트 시트** — `EffectAnimator`가 프레임 시트 소재를 단 `TextureRect`를 정해진 시간 동안 정해진 프레임 수만큼 진행합니다.
- **셰이더 없는 스프라이트 시트** — `EffectAnimNode`가 가로로 누운 스프라이트 시트를 훑고(프레임 너비 = 시트 너비 ÷ 프레임 수) 끝에서 스스로 사라집니다. 코드로 `EffectAnimNode.SpawnOn(target, sheet, frameCount, seconds)`로 만들 수도 있습니다.

프레임 세트와 코드만으로 쓰는 스프라이트 시트는 장면을 가득 채우도록 늘어나므로, 장면 크기에 맞춰 그린 프레임이 배경과 나란히 맞습니다.

### 코드 없는 첫 효과

가장 간단한 효과는 루트가 `TextureRect`인 장면에 그림 하나를 전체 화면으로 늘려 놓은 것입니다. 예컨대 반투명한 빛. `effect <이름>`으로 나타나고 대화가 끝나면 사라집니다.

## 문제가 생겼을 때

1. **아무것도 나타나지 않고 로그가 효과를 못 찾았다고 한다.** 파일이 게임에 포함된 모드의 `effects/<이름>.tscn`으로 존재하고, 시나리오의 이름과 일치해야 합니다.
2. **개발 중에는 되는데 플레이어에게는 안 된다.** 모드가 `.zip`으로 공개되었습니다. 폴더로 공개하세요.
3. **효과가 좀처럼 사라지지 않는다.** 그 대화가 끝날 때까지 남습니다. 대화를 끝내거나, 프레임 세트나 스프라이트 시트 조각으로 효과를 만드세요. 그것들은 끝나면 스스로를 지웁니다.

## 지속 환경 프로필

낙엽, 반딧불, 눈, 먼지, 비 같은 지속 환경 효과도 지원합니다. RPG Plus 프로필은 Village Pack에 있으며 다른 모드가 프로필과 방출기 유형을 추가할 수 있습니다. 좌표 투영을 제공하면 타일 맵 밖에서도 작동합니다.

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

프로필은 `atmosphere` 엔티티이며 맵 `Atmosphere`에 ID를 지정합니다. `particles`는 `flake`, `leaf`, `glow`, `dust`, `streak`를 지원합니다. `ripples`는 호스트의 `Region`(RPG는 `water` 제공)에 고리를 그립니다. 영역이 없으면 그리지 않습니다.

`Velocity`는 초당 월드 단위 XYZ이며 Z 양수가 위입니다. `Height`는 높이 범위, `Size`는 픽셀 반지름입니다. `Pulse`는 주기적 페이드, `Trail`은 잔상입니다. 이미터당 최대 512, 프로필당 2048입니다.

새 유형은 `Cthangover.Mods.Effects.Atmosphere`의 `IAtmosphereEmitterFactory`를 구현합니다. `Id`는 `Type`이 되고 `Create`는 `ExtraFields`를 가진 `AtmosphereEmitterInfo`를 받습니다. 이미터는 `Advance(delta, context)`, `Draw(canvas, context)`, `Dispose()`를 구현하고 `context.Project`, `Bounds`, `Regions`를 씁니다. 모드 로드·해제에 맞춰 등록하며 호스트가 인스턴스를 해제합니다.

맵 밖에서는 `AtmosphereView`의 `Configure(profileId, context)`, 프레임마다 `Advance(delta)`, 수동 해제 전 `Release()`를 호출합니다. RPG 의존성은 없습니다.
