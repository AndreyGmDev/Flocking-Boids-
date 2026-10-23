# Flocking-Boids-

INTEGRANTES:
Andrey Bispo Fernandes
Esther Souza Fernandes

INSTRUCOES PARA ABRIR A CENA PRINCIPAL
Assets\Scenes\Normal
Assets\Scenes\Predador

TABELA PARA OS PARAMETROS FINAIS: PESOS DAS TRES REGRAS, RAIOS, MAXSPEED E MAXFORCE

PARTE B - EXPERIMENTO DOCUMENTADO (RESPOSTAS)

1.Peso da separacao = 0: o que sobra do bando?
R: Os boids perdem o seu raio de distanciamento e comecam a grudar umas nas outras, formando aglomerados de boids que seguem juntos na mesma direcao (um tanto semelhante ao conceito de atomo de Dalton).

2.Peso do alinhamento = 0: o que acontece agora?
R: Diferente de antes que os boids caiam e andavam em sua maioria para as mesmas direcoes, agora os boids permanecem no mesmo lugar onde surgiram, e possuem pouca acao

3.Raio de percepcao 0,5 versus 5,0: como o formato do bando muda?
R: o raio de percepcao funciona como um circulo em volta do boid que permite que o mesmo veja seus iguais ao seu redor. Quanto maior for a percepcao (se colocarmos 5 por exemplo), maior e a distancia que mantem dos outros boids (uma observacao e que quando surgem, por precisarem manter mais distancia, geram uma sensacao de explosao). Porem, quanto menor a percepcao (colocando 0.5, por exemplo), a percepcao de um boid sobre os outros boids diminui, como se nao os visse la. Se o boid nao consegue ver outros boids, ele nao sente a necessidade de manter a distancia, e consequentemente, se mantem no mesmo lugar.

4.Predador: adicione uma esfera que persegue o bando e um comportamento de fuga com peso alto no ComputeForce. Descreva o efeito causado no grupo.
R: a prioridade principal dos boids para de ser manter distancia uns dos outros e se torna fugir do predador. Enqunto o predador tem como objetivo perseguir os outros boids.