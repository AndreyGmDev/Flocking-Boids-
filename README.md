
# Flocking-Boids

## Integrantes

-   Andrey Bispo Fernandes
-   Esther Souza Fernandes

## Instruções para abrir a cena principal

-   `Assets\Scenes\Normal`
-   `Assets\Scenes\Predador`

## Tabela de parâmetros finais

Tabela para registrar os parâmetros finais do experimento:

![FlockManager](https://github.com/AndreyGmDev/Flocking-Boids-/blob/main/imagens/Inspector_FlockManager.png)
![Boid](https://github.com/AndreyGmDev/Flocking-Boids-/blob/main/imagens/Inspector_Boid.png)

## Parte B — Experimento documentado (respostas)

### 1. Peso da separação = 0: o que sobra do bando?

**Resposta:** Os boids perdem seu raio de distanciamento e começam a se agrupar, formando aglomerados que seguem juntos na mesma direção. Isso é um tanto semelhante ao conceito do átomo de Dalton.

### 2. Peso do alinhamento = 0: o que acontece agora?

**Resposta:** Diferentemente do que acontecia antes, quando os boids se moviam predominantemente nas mesmas direções, agora eles permanecem próximos aos locais onde surgiram e apresentam pouca movimentação.

### 3. Raio de percepção 0,5 versus 5,0: como o formato do bando muda?

**Resposta:** O raio de percepção funciona como um círculo ao redor do boid, permitindo que ele perceba outros boids nas proximidades.

Quanto maior for o raio de percepção (por exemplo, 5,0), maior será a distância na qual o boid consegue perceber os outros. Uma observação é que, no momento em que surgem, os boids tendem a se afastar mais, gerando uma sensação de explosão.

Por outro lado, quanto menor for o raio de percepção (por exemplo, 0,5), menor será a capacidade de um boid perceber os demais, como se não os enxergasse. Se o boid não consegue perceber outros boids, ele não sente a necessidade de manter distância e, consequentemente, tende a permanecer no mesmo lugar.

### 4. Predador: adicione uma esfera que persegue o bando e um comportamento de fuga com peso alto no ComputeForce. Descreva o efeito causado no grupo.

**Resposta:** A principal prioridade dos boids deixa de ser manter distância uns dos outros e passa a ser fugir do predador. Enquanto isso, o predador tem como objetivo perseguir os demais boids, fazendo com que o grupo se movimente para tentar escapar.