using ProceduralMeshes;
using ProceduralMeshes.Generators;
using ProceduralMeshes.Streams;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour {

	public float perlinScale;
    public float waveSpeed;
    public float waveHeight;
    public float offset;

    public Mesh meshrender;
    public RenderTexture rendT;

    Vector3[] baseVertices;

    
	static MeshJobScheduleDelegate[] jobs = {		
		MeshJob<GeoIcosphere, PositionStream>.ScheduleParallel,
	};

	public enum MeshType {
		GeoIcosphere
	};

	MeshType meshType;

	MeshType geoIcosphere;


	[System.Flags]
	public enum MeshOptimizationMode {
		Nothing = 0, ReorderIndices = 1, ReorderVertices = 0b10
	}

	[SerializeField]
    MeshOptimizationMode meshOptimization;

	[SerializeField, Range(3, 50)]
	int resolution = 17;

	[System.Flags]
	public enum GizmoMode {
		Nothing = 0, Vertices = 1, Normals = 0b10, Tangents = 0b100, Triangles = 0b1000
	}

	[SerializeField]
	GizmoMode gizmos;

	

	Mesh mesh;

	[System.NonSerialized]
	Vector3[] vertices, normals;

	[System.NonSerialized]
	Vector4[] tangents;

	[System.NonSerialized]
	int[] triangles;

	 private void OnEnable()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;
      
    }

	private void Start() {
 
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;
        meshrender = mesh;
        }

	void Awake () {
		mesh = new Mesh {
			name = "Hautball"
		};
		GetComponent<MeshFilter>().mesh = mesh;
	}

	void OnDrawGizmos () {
		if (gizmos == GizmoMode.Nothing || mesh == null) {
			return;
		}

		bool drawVertices = (gizmos & GizmoMode.Vertices) != 0;
		bool drawNormals = (gizmos & GizmoMode.Normals) != 0;
		bool drawTangents = (gizmos & GizmoMode.Tangents) != 0;
		bool drawTriangles = (gizmos & GizmoMode.Triangles) != 0;

		if (vertices == null) {
			vertices = mesh.vertices;
		}
		if (drawNormals && normals == null) {
			drawNormals = mesh.HasVertexAttribute(VertexAttribute.Normal);
			if (drawNormals) {
				normals = mesh.normals;
			}
		}
		if (drawTangents && tangents == null) {
			drawTangents = mesh.HasVertexAttribute(VertexAttribute.Tangent);
			if (drawTangents) {
				tangents = mesh.tangents;
			}
		}
		if (drawTriangles && triangles == null) {
			triangles = mesh.triangles;
		}

		Transform t = transform;
		for (int i = 0; i < vertices.Length; i++) {
			Vector3 position = t.TransformPoint(vertices[i]);
			if (drawVertices) {
				Gizmos.color = Color.white;
				Gizmos.DrawSphere(position, 0.02f);
			}
			if (drawNormals) {
				Gizmos.color = Color.green;
				Gizmos.DrawRay(position, t.TransformDirection(normals[i]) * 0.2f);
			}
			if (drawTangents) {
				Gizmos.color = Color.red;
				Gizmos.DrawRay(position, t.TransformDirection(tangents[i]) * 0.2f);
			}
		}

		if (drawTriangles) {
			float colorStep = 1f / (triangles.Length - 3);
			for (int i = 0; i < triangles.Length; i += 3) {
				float c = i * colorStep;
				Gizmos.color = new Color(c, 0f, c);
				Gizmos.DrawSphere(
					t.TransformPoint((
						vertices[triangles[i]] +
						vertices[triangles[i + 1]] +
						vertices[triangles[i + 2]]
					) * (1f / 3f)),
					0.02f
				);
			}
		}
	}

	void OnValidate () => enabled = true;

	void Update () {
		GenerateMesh();
		enabled = true;

		vertices = null;
		normals = null;
		tangents = null;
		triangles = null;

	
	}

	void GenerateMesh () {
		Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
		Mesh.MeshData meshData = meshDataArray[0];
		
		//generate geoicosphere
		
		jobs[(int)geoIcosphere](mesh, meshData, resolution, default).Complete();

		Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
		 
	    //perlin noise

        Vector3[] vertices = mesh.vertices;
        mesh.RecalculateNormals();

        Vector3[] verts = mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            float pX = (verts[i].x * perlinScale) + (Time.timeSinceLevelLoad * waveSpeed) + offset;
            float pZ = (verts[i].z * perlinScale) + (Time.timeSinceLevelLoad * waveSpeed) + offset;
            verts[i] = verts[i] + ((Mathf.PerlinNoise(pX, pZ)) * waveHeight * mesh.normals[i].normalized);
        }

        mesh.vertices = verts;

		
		

	}


       
        //GetComponent<MeshFilter>().sharedMesh = mesh;
}
