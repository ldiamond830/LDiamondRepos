#include "Octant.h"
using namespace BTX;
//  Octant
uint Octant::m_uOctantCount = 0;
uint Octant::m_uMaxLevel = 3;
uint Octant::m_uIdealEntityCount = 5;
Octant::Octant(uint a_nMaxLevel, uint a_nIdealEntityCount)
{
	
	// This constructor is meant to be used ONLY on the root node
	
	Init();
	m_uOctantCount = 0;
	m_uMaxLevel = a_nMaxLevel;
	m_uIdealEntityCount = a_nIdealEntityCount;
	m_uID = m_uOctantCount;

	m_pRoot = this;
	m_lChild.clear();

	//create a rigid body that encloses all the objects in this octant
	std::vector<vector3> lMinMax;

	//gets the max and min values of each entity
	for (uint i = 0; i < m_pEntityMngr->GetEntityCount(); i++)
	{
		RigidBody* temp = m_pEntityMngr->GetEntity(i)->GetRigidBody();
		lMinMax.push_back(temp->GetMaxGlobal());
		lMinMax.push_back(temp->GetMinGlobal());
	}

	RigidBody pRigidBody = RigidBody(lMinMax);


	
	m_fSize = pRigidBody.GetHalfWidth().x * 2.0f;
	m_v3Center = pRigidBody.GetCenterLocal();
	m_v3Min = m_v3Center - pRigidBody.GetHalfWidth();
	m_v3Max = m_v3Center + pRigidBody.GetHalfWidth();

	m_uOctantCount++; 
	ConstructTree(m_uMaxLevel); 
}

bool Octant::IsColliding(uint a_uRBIndex)
{
	//Get how many objects there are in the world
	//If the index given is larger than the number of elements in the bounding object there is no collision
	if (a_uRBIndex > m_pEntityMngr->GetEntityCount()) {
		return false;
	}

	bool bColliding = true;

	//gets the max and min the current entity being tested
	RigidBody* currentEntityBody = m_pEntityMngr->GetEntity(a_uRBIndex)->GetRigidBody();
	vector3 otherMin = currentEntityBody->GetMinGlobal();
	vector3 otherMax = currentEntityBody->GetMaxGlobal();

	if (m_v3Max.x < otherMin.x) //checks to the right of other
		bColliding = false;
	if (m_v3Min.x > otherMax.x) //checks to the left of other
		bColliding = false;

	if (m_v3Max.y < otherMin.y) //checks below of other
		bColliding = false;
	if (m_v3Min.y > otherMax.y) //checks above of other
		bColliding = false;

	if (m_v3Max.z < otherMin.z) //checks behind of other
		bColliding = false;
	if (m_v3Min.z > otherMax.z) //checks in front of other
		bColliding = false;


	//if none of the if statements are called returns as colliding
	return bColliding;
}
void Octant::Display(uint a_nIndex, vector3 a_v3Color)
{
	//if the index is greater than the number of octants displays all
	if (a_nIndex > m_lChild.size()) {
		m_pRoot->Display(C_YELLOW);
	}

	// Displays the specified octant
	m_pModelMngr->AddWireCubeToRenderList(glm::translate(IDENTITY_M4, m_lChild[a_nIndex]->m_v3Center) * glm::scale(vector3(m_lChild[a_nIndex]->m_fSize)), a_v3Color);
}
void Octant::Display(vector3 a_v3Color)
{
	//adds the current octant to the render list
	m_pModelMngr->AddWireCubeToRenderList(glm::translate(IDENTITY_M4, m_v3Center) *
		glm::scale(vector3(m_fSize)), a_v3Color);

	//moves recursively through the tree until all octants have been added to render list
	if (m_uChildren != 0) {
		for (int i = 0; i < 8; i++) {
			m_pChild[i]->Display();
		}
	}
}
void Octant::Subdivide(void)
{
	//If this node has reach the maximum depth return without changes
	if (m_uLevel >= m_uMaxLevel)
		return;

	//If this node has been already subdivided return without changes
	if (m_uChildren != 0)
		return;


	m_uChildren = 8;

	//Subdivide the space and allocate 8 children
	//top left front
	m_pChild[0] = (new Octant(vector3(m_v3Center.x - (m_fSize / 4), m_v3Center.y + (m_fSize / 4), m_v3Center.z + (m_fSize / 4)), m_fSize / 2));


	//top right front
	m_pChild[1] = (new Octant(vector3(m_v3Center.x + (m_fSize / 4), m_v3Center.y + (m_fSize / 4), m_v3Center.z + (m_fSize / 4)), m_fSize / 2));

	//bottom left front
	m_pChild[2] = (new Octant(vector3(m_v3Center.x - (m_fSize / 4), m_v3Center.y - (m_fSize / 4), m_v3Center.z + (m_fSize / 4)), m_fSize / 2));


	//bottom right front
	m_pChild[3] = (new Octant(vector3(m_v3Center.x + (m_fSize / 4), m_v3Center.y - (m_fSize / 4), m_v3Center.z + (m_fSize / 4)), m_fSize / 2));



	//top left back
	m_pChild[4] = (new Octant(vector3(m_v3Center.x - (m_fSize / 4), m_v3Center.y + (m_fSize / 4), m_v3Center.z - (m_fSize / 4)), m_fSize / 2));



	//top right back
	m_pChild[5] = (new Octant(vector3(m_v3Center.x + (m_fSize / 4), m_v3Center.y + (m_fSize / 4), m_v3Center.z - (m_fSize / 4)), m_fSize / 2));



	//bottom left back
	m_pChild[6] = (new Octant(vector3(m_v3Center.x - (m_fSize / 4), m_v3Center.y - (m_fSize / 4), m_v3Center.z - (m_fSize / 4)), m_fSize / 2));



	//bottom right back
	m_pChild[7] = (new Octant(vector3(m_v3Center.x + (m_fSize / 4), m_v3Center.y - (m_fSize / 4), m_v3Center.z - (m_fSize / 4)), m_fSize / 2));



	//populates variables not handled by constructor for each child
	for (uint i = 0; i < 8; i++) {
		m_pChild[i]->m_pParent = this;
		m_pChild[i]->m_pRoot = m_pChild[i]->m_pParent->m_pRoot;
		m_pChild[i]->m_uLevel = m_uLevel + 1;
		m_pRoot->m_lChild.push_back(m_pChild[i]);
	}

	//checks if the children need to be subdivided
	if (m_uChildren != 0) {
		for (uint i = 0; i < 8; i++) {
			if (m_pChild[i]->ContainsAtLeast(m_uIdealEntityCount)) {
				m_pChild[i]->Subdivide();
			}
		}
	}
}
bool Octant::ContainsAtLeast(uint a_nEntities)
{
	//checks how many entity objects live within this octant
	uint count = 0;
	for (uint i = 0; i < m_pEntityMngr->GetEntityCount(); i++) {
		if (IsColliding(i)) {

			count++;
		}
	}

	return count > a_nEntities;
}
void Octant::AssignIDtoEntity(void)
{
	//Recursive method
	//Have to traverse the tree and make sure to tell the entity manager
	//what octant (space) each object is at

	//checks if the current octant has children
	if (m_uChildren != 0) {
		for (int i = 0; i < 8; i++) {
			//if so recursively travels down the tree
			m_pChild[i]->AssignIDtoEntity();
		}
	}
	//once the function recurses down to the leaves assigns each entity in their list with the current ID
	else {
		for (int i = 0; i < m_EntityList.size(); i++)
		{
			m_pEntityMngr->AddDimension(m_EntityList[i], m_uID);
		}
	}


}
//helper methods and set up provided to me as starter code, I only modified ConstructTree() 
void Octant::Init(void)
{
	m_uChildren = 0;

	m_fSize = 0.0f;

	m_uID = m_uOctantCount;
	m_uLevel = 0;

	m_v3Center = vector3(0.0f);
	m_v3Min = vector3(0.0f);
	m_v3Max = vector3(0.0f);

	m_pModelMngr = ModelManager::GetInstance();
	m_pEntityMngr = EntityManager::GetInstance();

	m_pRoot = nullptr;
	m_pParent = nullptr;
	for (uint n = 0; n < 8; n++)
	{
		m_pChild[n] = nullptr;
	}
}
void Octant::Swap(Octant& other)
{
	std::swap(m_uChildren, other.m_uChildren);

	std::swap(m_fSize, other.m_fSize);
	std::swap(m_uID, other.m_uID);
	std::swap(m_pRoot, other.m_pRoot);
	std::swap(m_lChild, other.m_lChild);

	std::swap(m_v3Center, other.m_v3Center);
	std::swap(m_v3Min, other.m_v3Min);
	std::swap(m_v3Max, other.m_v3Max);

	m_pModelMngr = ModelManager::GetInstance();
	m_pEntityMngr = EntityManager::GetInstance();

	std::swap(m_uLevel, other.m_uLevel);
	std::swap(m_pParent, other.m_pParent);
	for (uint i = 0; i < 8; i++)
	{
		std::swap(m_pChild[i], other.m_pChild[i]);
	}
}
void Octant::Release(void)
{
	//this is a special kind of release, it will only happen for the root
	if (m_uLevel == 0)
	{
		KillBranches();
	}
	m_uChildren = 0;
	m_fSize = 0.0f;
	m_EntityList.clear();
	m_lChild.clear();
}
void Octant::ConstructTree(uint a_nMaxLevel)
{
	//If this method is tried to be applied to something else
	//other than the root, don't.
	if (m_uLevel != 0)
		return;

	m_uMaxLevel = a_nMaxLevel; //Make sure you mark which is the maximum level you are willing to reach
	m_uOctantCount = 1;// We will always start with one octant

	//If this was initialized before make sure its empty
	m_EntityList.clear();//Make sure the list of entities inside this octant is empty
	KillBranches();
	m_lChild.clear();
	//stores the root in the child list so it can be displayed by index
	m_lChild.push_back(m_pRoot);
	//If we have more entities than those that we ideally want in this octant we subdivide it
	if (ContainsAtLeast(m_uIdealEntityCount))
	{
		Subdivide();
	}



	for (int i = 0; i < m_pRoot->m_lChild.size(); i++) {
		//sets the ID of each child in the list ID 0 is the root
		m_pRoot->m_lChild[i]->m_uID = i + 1;

		//populates the entity list of each child
		for (int j = 0; j < m_pEntityMngr->GetEntityCount(); j++) {
			if (m_pRoot->m_lChild[i]->IsColliding(j)) {

				m_pRoot->m_lChild[i]->m_EntityList.push_back((uint)j);
			}
		}
	}

	//sets the root ID;
	m_pRoot->m_uID = 0;

	//resets the dimension list of each entity so that the don't have the previous leaf IDs after subdividing
	for (uint i = 0; i < m_pEntityMngr->GetEntityCount(); i++) {
		for (uint j = 0; j < m_lChild.size(); j++) {
			m_pEntityMngr->RemoveDimension(i, j);
		}
	}

	AssignIDtoEntity();//Add octant ID to Entities
	ConstructList();//construct the list of objects
}
//The big 3
Octant::Octant(vector3 a_v3Center, float a_fSize)
{
	//Will create the octant object based on the center and size but will not construct children
	Init();
	m_v3Center = a_v3Center;
	m_fSize = a_fSize;

	m_v3Min = m_v3Center - (vector3(m_fSize) / 2.0f);
	m_v3Max = m_v3Center + (vector3(m_fSize) / 2.0f);

	m_uOctantCount++;


	//AssignIDtoEntity();//Add octant ID to Entities
}
Octant::Octant(Octant const& other)
{
	m_uChildren = other.m_uChildren;
	m_v3Center = other.m_v3Center;
	m_v3Min = other.m_v3Min;
	m_v3Max = other.m_v3Max;

	m_fSize = other.m_fSize;
	m_uID = other.m_uID;
	m_uLevel = other.m_uLevel;
	m_pParent = other.m_pParent;

	m_pRoot, other.m_pRoot;
	m_lChild, other.m_lChild;

	m_pModelMngr = ModelManager::GetInstance();
	m_pEntityMngr = EntityManager::GetInstance();

	for (uint i = 0; i < 8; i++)
	{
		m_pChild[i] = other.m_pChild[i];
	}
}
Octant& Octant::operator=(Octant const& other)
{
	if (this != &other)
	{
		Release();
		Init();
		Octant temp(other);
		Swap(temp);
	}
	return *this;
}
Octant::~Octant() { Release(); };
//Accessors
float Octant::GetSize(void) { return m_fSize; }
vector3 Octant::GetCenterGlobal(void) { return m_v3Center; }
vector3 Octant::GetMinGlobal(void) { return m_v3Min; }
vector3 Octant::GetMaxGlobal(void) { return m_v3Max; }
uint Octant::GetOctantCount(void) { return m_uOctantCount; }
bool Octant::IsLeaf(void) { return m_uChildren == 0; }
Octant* Octant::GetParent(void) { return m_pParent; }
//--- other methods
Octant* Octant::GetChild(uint a_nChild)
{
	//if its asking for more than the 8th children return nullptr, as we don't have one
	if (a_nChild > 7) return nullptr;
	return m_pChild[a_nChild];
}
void Octant::KillBranches(void)
{
	/*This method has recursive behavior that is somewhat hard to explain line by line but
	it will traverse the whole tree until it reaches a node with no children and
	once it returns from it to its parent it will kill all of its children, recursively until
	it reaches back the node that called it.*/
	for (uint nIndex = 0; nIndex < m_uChildren; nIndex++)
	{
		m_pChild[nIndex]->KillBranches();
		delete m_pChild[nIndex];
		m_pChild[nIndex] = nullptr;
	}
	m_uChildren = 0;
}
void Octant::DisplayLeaves(vector3 a_v3Color)
{
	/*
	* Recursive method
	* it will traverse the tree until it find leaves and once found will draw them
	*/
	uint nLeafs = m_lChild.size(); //get how many children this will have (either 0 or 8)
	for (uint nChild = 0; nChild < nLeafs; nChild++)
	{
		m_lChild[nChild]->DisplayLeaves(a_v3Color);
	}
	//Draw the cube
	m_pModelMngr->AddWireCubeToRenderList(glm::translate(IDENTITY_M4, m_v3Center) *
		glm::scale(vector3(m_fSize)), a_v3Color);
}
void Octant::ClearEntityList(void)
{
	/*
	* Recursive method
	* It will traverse the tree until it find leaves and once found will clear its content
	*/
	for (uint nChild = 0; nChild < m_uChildren; nChild++)
	{
		m_pChild[nChild]->ClearEntityList();
	}
	m_EntityList.clear();
}
void Octant::ConstructList(void)
{
	/*
	* Recursive method
	* It will traverse the tree adding children
	*/
	for (uint nChild = 0; nChild < m_uChildren; nChild++)
	{
		m_pChild[nChild]->ConstructList();
	}
	//if we find a non-empty child add it to the tree
	if (m_EntityList.size() > 0)
	{
		m_pRoot->m_lChild.push_back(this);
	}
}

